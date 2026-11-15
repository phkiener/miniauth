using System.Runtime.InteropServices;

namespace LibC;

/// <summary>
/// Utilities to retrieve the groups of a user.
/// </summary>
public static class Groups
{
    /// <summary>
    /// Enumerate all groups of the given user.
    /// </summary>
    /// <param name="user">The user whose groups to enumerate.</param>
    /// <returns>The groups that the given user is a member of.</returns>
    /// <seealso cref="EnumerateGroups(int)"/>
    public static IEnumerable<string> EnumerateGroups(string user)
    {
        var userInfoPointer = Native.getpwnam(user);
        var userInfo = Marshal.PtrToStructure<Native.passwd>(userInfoPointer);

        return EnumerateGroups(userInfo.pw_name, userInfo.pw_gid);
    }

    /// <summary>
    /// Enumerate all groups of the given user.
    /// </summary>
    /// <param name="userId">Id of the user whose groups to enumerate.</param>
    /// <returns>The groups that the given user is a member of.</returns>
    /// <seealso cref="EnumerateGroups(string)"/>
    public static IEnumerable<string> EnumerateGroups(int userId)
    {
        var userInfoPointer = Native.getpwuid(userId);
        var userInfo = Marshal.PtrToStructure<Native.passwd>(userInfoPointer);

        return EnumerateGroups(userInfo.pw_name, userInfo.pw_gid);
    }

    private static IEnumerable<string> EnumerateGroups(string username, int userGroupId)
    {
        var groupNumber = 16; // start with enough space so that we can skip the second call in most cases
        var groupPointer = Marshal.AllocHGlobal(sizeof(int) * groupNumber);
        var groups = Native.getgrouplist(username, userGroupId, groupPointer, ref groupNumber);

        if (groups is -1)
        {
            Marshal.FreeHGlobal(groupPointer);

            groupPointer = Marshal.AllocHGlobal(sizeof(int) * groupNumber);
            var result = Native.getgrouplist(username, userGroupId, groupPointer, ref groupNumber);
            if (result is not 0)
            {
                throw new InvalidOperationException("Failed to retreive group list");
            }
        }

        for (var i = 0; i < groupNumber; ++i)
        {
            var groupId = Marshal.ReadInt32(groupPointer + sizeof(int) * i);
            if (groupId is 0)
            {
                continue;
            }

            var groupInfoPointer = Native.getgrgid(groupId);
            var groupInfo = Marshal.PtrToStructure<Native.group>(groupInfoPointer);

            yield return groupInfo.gr_name;
        }

        Marshal.FreeHGlobal(groupPointer);
    }
}
