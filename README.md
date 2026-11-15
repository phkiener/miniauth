# MiniAuth

A tiny and unambitious authentication server. The idea is to use it with an
authentication-aware reverse proxy like Caddy. The proxy can then copy the
specified headers for the downstream service so that the user is automagically
authenticted. Magic!

Technically, any "identity provider" can be supported, but the initial case was
to use MiniAuth together with PAM: Pluggable Authentication Modules. I wanted to
be able to use the system user accounts directly for other applications, PAM is
the way to go here.

## Installation

Two applications are provided:

- `MiniAuth.Host` as web UI
- `MiniAuth.PAM` as proxy application to handle the PAM authentication

`MiniAuth.PAM` is a single binary meant to be installed directly on the host. Run
it as root or with `setuid` for root and set the PAM-service and socket file to use:

```sh
miniauth-pam passwd /etc/miniauth/pam.socket
```

It's recommended to limit readability of `/etc/miniauth` so that not everyone can
check crdentials of every user. This is why it needs to run as root by the way;
PAM can only validate the current user when run as non-root.

Start the web UI with `docker`, `podman` or a systemd quadlet. Configure the
following environment variables:

- `MINIAUTH_IDENTITY=PAM` to enable PAM authentification
- `PAM_SOCKET=/etc/miniauth/pam.socket` for the socket to use; this needs to be mounted in the container
- `MINIAUTH_DOMAIN=your-domain.com` for the (public) domain to use for the session cookie; everything you host should be a subdomain of that one
- `MINIAUTH_LOGINHOST=https://auth.your-domain.com` for the public URL for the web UI
- `MINIAUTH_SESSIONLIFETIMEDAYS=7` for how long the session should be valid for (1-14 days)

The user will receive a session cookie containing the username plus all groups
that the user is a member of. It's not the prettiest, but it works.
