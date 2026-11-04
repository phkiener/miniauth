using System.Diagnostics.CodeAnalysis;

namespace Miniauth.Session;

public sealed class UserSession(string user, IEnumerable<string> scopes)
{
    public string User { get; } = user;
    public string[] Scopes { get; } = scopes.ToArray();

    public string Serialize()
    {
        return $"{User}::{string.Join(":", Scopes)}";
    }

    public static bool TryParse(string text, [NotNullWhen(true)] out UserSession? session)
    {
        var parts = text.Split("::", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (parts is [var username])
        {
            session = new UserSession(username, []);
            return true;
        }

        if (parts is [var name, var scopes])
        {
            var parsedScopes = scopes.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            session = new UserSession(name, parsedScopes);
            return true;
        }

        session = null;
        return false;
    }
}
