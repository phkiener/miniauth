# Miniauth

A tiny and unambitious authentication server. The idea is to use it with an
authentication-aware reverse proxy like Caddy. The proxy can then copy the
specified headers for the downstream service so that the user is automagically
authenticted. Magic!

## Mechanism

Two applications are provided:

- `Miniauth` as web UI
- `Miniauth.Check` as CLI to check the credentials

To check credentials, access to `/etc/shadow` is needed - which is limited to
`root`, usually. To handle this, `chown root` the assembly built for `Miniauth.Check`
and `chmod u+s` it. You should also `chmod o-rx`, just to be sure.

Start `Miniauth` with `DOTNET_URLS=<your-url> MINIAUTH_CHECK=<path-to-check-program>`
as environment variables and you're good to go.
