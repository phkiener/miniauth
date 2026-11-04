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
        if (parts is not [var name, var scopeList])
        {
            session = null;
            return false;
        }

        var scopes = scopeList.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        session = new UserSession(name, scopes);
        return true;
    }
}
