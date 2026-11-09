using System.Diagnostics.CodeAnalysis;

namespace Miniauth.Check.Files;

public static class ShadowFile
{
    private sealed record ShadowFileEntry(string UserName, string PasswordHash);

    public static string? FindPasswordHash(string username, string path)
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
        // /etc/shadow has 9 fields, each separated by a colon (':').
        // We only care about the first two, but we'll reject anything that doesn't conform to shadow(5).

        var parts = line.Split(':');
        if (parts is not [var login, var hash, _, _, _, _, _, _, _])
        {
            entry = null;
            return false;
        }

        entry = new ShadowFileEntry(login, hash);
        return true;
    }
}
