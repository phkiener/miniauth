# Full example

My own setup that's working fine for me. Might be weird at some parts or convoluted at others, but it works.

## miniauth-pam

The helper binary is run as systemd service for root.

1. Copy `miniauth-pam` to `/usr/bin/miniauth-pam`
   * `chown root:root`
   * `chmod 755`
2. For SELinux, set the `bin_t` type on it: `fcontext -a -f a -t bin_t -r 's0' '/usr/bin/miniauth-pam'`
3. Create a group `pam-proxy` that will control access to the socket exposed by the systemd service
4. Create the directory `/etc/miniauth` where the socket will be created
   * `chown root:pam-proxy`
   * `chmod 750`
5. Add the user that will run the web UI (direct or container) to `pam-proxy`
   * `loginctl terminate-user` that user and login again to make sure systemd picks up the new role
6. Create `miniauth-pam.service` in `/etc/systemd/system/`
   * Set `User` to `root` because only root can authenticate *other* users via PAM
   * Set `Group` to `pam-proxy` so that the created socket file will be owned by `root:pam-proxy`
   * Set `UMask` to `0017` so that the create socket file will be group-writeable - which is needed to connect to the socket - but inaccessible to everyone else
7. `systemctl enable miniauth-pam.service` and the socket should be up and running!
   * Try `nc -U /etc/miniauth/pam.socket` with a user that's part of `pam-proxy` to see that permissions are set correctly

`/etc/systemd/system/miniauth-pam.service`:
```ini
[Unit]
Description=MiniAuth PAM Proxy

[Service]
Type=simple
ExecStart=/usr/bin/miniauth-pam password-auth /etc/miniauth/pam.socket
User=root
Group=pam-proxy
UMask=0017

[Install]
WantedBy=multi-user.target
```

## Web UI

The web UI is started as quadlet (systemd service using podman) from a dedicated non-root service user.

1. Make sure the user has systemd setup and is part of `pam-proxy`
2. Create `miniauth.container` in `.config/containers/systemd`
   * Make sure the running container inherts access to `/etc/miniauth/pam.socket`...
   * Set `UserNS` to `keep-id`
   * Set `GroupAdd` to `keep-groups`
   * Set `PodmanArgs` to `--annotation=run.oci.keep_original_groups=1`
   * Set `MINIAUTH_DOMAIN` to your local domain (e.g. `my-domain.com`)
   * Set `MINIAUTH_LOGINHOST` to the public URL that can be used to reach the web UI (e.g. `https://auth.my-domain.com`)
3. `systemctl enable miniauth.service` and the UI should be up and running!

`.config/containers/systemd/miniauth.container`:
```ini
[Unit]
Description=MiniAuth Web UI

[Container]
Image=ghcr.io/phkiener/miniauth:0.1.2 # ...or whatever is the latest version

SecurityLabelDisable=true # SELinux is a pain with containers and shared files

DNS=9.9.9.9
PublishPort=15151:5000

UserNS=keep-id
GroupAdd=keep-groups
PodmanArgs=--annotation=run.oci.keep_original_groups=1

Volume=/etc/miniauth/:/etc/miniauth/
Environment="DOTNET_URLS=http://*:5000"
Environment="MINIAUTH_IDENTITY=PAM"
Environment="PAM_SOCKET=/etc/miniauth/pam.socket"
Environment="MINIAUTH_DOMAIN=local.phkiener.ch"
Environment="MINIAUTH_LOGINHOST=https://auth.local.phkiener.ch"
Environment="MINIAUTH_SESSIONLIFETIMEDAYS=10"

[Service]
Restart=always

[Install]
WantedBy=default.target
```

## Caddy

Caddy is running as systemd-service for root.

1. Add a section to the Caddyfile to route to the running web UI
   * Basically `reverse_proxy http://host.containers.internal:15151`
2. For every service that you want to protect, add a `forward_auth` section
3. `systemctl reload-or-restart caddy.service` and it's done

`/etc/caddy/Caddyfile`:
```
# Example service
service.local.phkiener.ch {
    import certificate # tls termination, don't mind me

    forward_auth @protected host.containers.internal:15151 {
        # scope=<group> if the user needs to be member of the given group;
        # skip ?scope if you just want the user to be logged in
        uri /challenge?scope=<group>

        copy_headers Remote-User # depends on the service
    }

    reverse_proxy host.containers.internal:9999
}

# MiniAuth web UI
auth.local.phkiener.ch {
    import certificate # tls termination, don't mind me

    reverse_proxy host.containers.internal:15151
}
```
