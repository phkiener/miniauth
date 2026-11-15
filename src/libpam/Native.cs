using System.Runtime.InteropServices;
using LibPAM.Types;

namespace LibPAM;

internal static partial class Native
{
    // libpam is versioned only (on my system at least), DllImport doesn't find it without the suffix.
#if LINUX
    private const string Library = "libpam.so.0";
#elif OSX
    private const string Library = "libpam";
#else
    #error PAM is not supported on this platform
#endif

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate Status pam_conv_callback(int num_msg, ref IntPtr msg, ref IntPtr resp, IntPtr appdata);

    [StructLayout(LayoutKind.Sequential)]
    public struct pam_conv
    {
        public IntPtr conv;
        public IntPtr appdata_ptr;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct pam_message
    {
        public MessageStyle msg_style;
        [MarshalAs(UnmanagedType.LPStr)] public string msg;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct pam_response
    {
        [MarshalAs(UnmanagedType.LPStr)] public string resp;
        public int resp_retcode;
    }
    [LibraryImport(Library, EntryPoint = "pam_start")]
    public static partial Status pam_start(
        [MarshalAs(UnmanagedType.LPStr)] string service,
        [MarshalAs(UnmanagedType.LPStr)] string? user,
        IntPtr pam_conversation,
        ref IntPtr pamh);

    [LibraryImport(Library, EntryPoint = "pam_authenticate")]
    public static partial Status pam_authenticate(IntPtr pamh, Flags flags);

    [LibraryImport(Library, EntryPoint = "pam_acct_mgmt")]
    public static partial Status pam_acct_mgmt(IntPtr pamh, Flags flags);

    [LibraryImport(Library, EntryPoint = "pam_get_item")]
    public static partial Status pam_get_item(IntPtr pamh, ItemType item_type, out IntPtr item);

    [LibraryImport(Library, EntryPoint = "pam_set_item")]
    public static partial Status pam_set_item(IntPtr pamh, ItemType item_type, IntPtr item);

    [LibraryImport(Library, EntryPoint = "pam_end")]
    public static partial Status pam_end(IntPtr pamh, Status status);
}
