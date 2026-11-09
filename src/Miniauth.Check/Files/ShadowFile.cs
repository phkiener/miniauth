using System.Diagnostics.CodeAnalysis;

namespace Miniauth.Check.Files;

public static class ShadowFile
{
    private sealed record ShadowFileEntry(string UserName, string PasswordHash);

    public static string? FindPasswordHash(string username, string path = "/etc/shadow")
    {
        foreach (var line in File.ReadAllLines(path))
        {
            if (TryParse(line, out var entry) && entry.UserName == username)
            {
                return entry.PasswordHash;
            }
        }

        return null;
    }

    private static bool TryParse(string line, [MaybeNullWhen(false)] out ShadowFileEntry entry)
    {
        var parts = line.Split(':');
        if (parts is not [var login, var hash, ..])
        {
            entry = null;
            return false;
        }

        entry = new ShadowFileEntry(login, hash);
        return true;
    }
}
