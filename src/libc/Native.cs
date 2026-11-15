using System.Runtime.InteropServices;

namespace LibC;

internal static partial class Native
{
    private const string Library = "libc";

    [StructLayout(LayoutKind.Sequential)]
    public struct passwd
    {
        [MarshalAs(UnmanagedType.LPStr)] public string pw_name;
        [MarshalAs(UnmanagedType.LPStr)] public string? pw_passwd;
        public int pw_uid;
        public int pw_gid;
        [MarshalAs(UnmanagedType.LPStr)] public string? pw_gecos;
        [MarshalAs(UnmanagedType.LPStr)] public string pw_dir;
        [MarshalAs(UnmanagedType.LPStr)] public string pw_shell;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct group
    {
        [MarshalAs(UnmanagedType.LPStr)] public string gr_name;
        [MarshalAs(UnmanagedType.LPStr)] public string? gr_passwd;
        public int gr_gid;
        public IntPtr gr_mem;
    }

    [LibraryImport(Library, EntryPoint = "getpwnam")]
    public static partial IntPtr getpwnam([MarshalAs(UnmanagedType.LPStr)] string name);

    [LibraryImport(Library, EntryPoint = "getpwuid")]
    public static partial IntPtr getpwuid(int uid);

    [LibraryImport(Library, EntryPoint = "getgrgid")]
    public static partial IntPtr getgrgid(int gid);

    [LibraryImport(Library, EntryPoint = "getgrouplist")]
    public static partial int getgrouplist(
        [MarshalAs(UnmanagedType.LPStr)] string user,
        int group,
        IntPtr groups,
        ref int ngroups);
}
