using System.Diagnostics.CodeAnalysis;

namespace Miniauth.Check.Files;

public sealed record GroupFileEntry(string GroupName, string[] Users);

public static class GroupFile
{
    public static IEnumerable<GroupFileEntry> Read(string path = "/etc/shadow")
    {
        foreach (var line in File.ReadAllLines(path))
        {
            if (TryParse(line, out var entry))
            {
                yield return entry;
            }
        }
    }

    private static bool TryParse(string line, [MaybeNullWhen(false)] out GroupFileEntry entry)
    {
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
