using System.Diagnostics.CodeAnalysis;

namespace Miniauth.Check.Files;

public static class GroupFile
{
    private sealed record GroupFileEntry(string GroupName, string[] Users);

    public static IEnumerable<string> FindGroups(string username, string path)
    {
        foreach (var line in File.ReadAllLines(path))
        {
            if (TryParse(line, out var entry) && entry.Users.Contains(username))
            {
                yield return entry.GroupName;
            }
        }
    }

    private static bool TryParse(string line, [MaybeNullWhen(false)] out GroupFileEntry entry)
    {
        // /etc/shadow has 4 fields, each separated by a colon (':').
        // We only care about the first and last, but we'll reject anything that doesn't conform to group(5).

        var parts = line.Split(':');
        if (parts is not [var group, _, _, var users])
        {
            entry = null;
            return false;
        }

        entry = new GroupFileEntry(group, users.Split(','));
        return true;
    }
}
