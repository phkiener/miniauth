using System.Diagnostics.CodeAnalysis;

namespace Miniauth.Check.Files;

public sealed record ShadowFileEntry(string UserName, string PasswordHash);

public static class ShadowFile
{
    public static IEnumerable<ShadowFileEntry> Read(string path = "/etc/shadow")
    {
        foreach (var line in File.ReadAllLines(path))
        {
            if (TryParse(line, out var entry))
            {
                yield return entry;
            }
        }
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
