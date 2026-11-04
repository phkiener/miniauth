# Miniauth

A tiny and unambitious authentication server. The idea is to use it with an
authentication-aware reverse proxy like Caddy. The proxy can then copy the
specified headers for the downstream service so that the user is automagically
authenticted. Magic!
