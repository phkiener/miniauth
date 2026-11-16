# How to upgrade

*For now*, both the web UI and `miniauth-pam` need to be the same version, I'm not doing any
versioning scheme for the socket API for now. Maybe in the future, who knows...

## miniauth-pam

Download the latest executable from GitHub Releases.
Then run:

```sh
sudo chmod --reference=/usr/bin/miniauth-pam miniauth-pam
sudo chown --reference=/usr/bin/miniauth-pam miniauth-pam
sudo mv miniauth-pam /usr/bin/miniauth-pam
sudo restorecon /usr/bin/miniauth-pam # SELinux needs to pick up bin_t again
sudo systemctl reload-or-restart miniauth-pam.service
```

## Web UI

Navigate to `.config/containers/systemd/miniauth.container` and update the image tag.
Then run:

```sh
systemctl --user daemon-reload
systemctl --user reload-or-restart miniauth.service
```
