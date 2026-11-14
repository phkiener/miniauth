using System.Runtime.InteropServices;

namespace LibPAM.Types;

internal static partial class Native
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate Status pam_conv_callback_raw(
        int num_msg,
        IntPtr msg,
        ref IntPtr resp,
        IntPtr appdata_ptr);

    public delegate Status pam_conv_callback(
        int num_msg,
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] pam_message[] msg,
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] ref pam_response[] resp,
        ref IntPtr appdata_ptr);

    [StructLayout(LayoutKind.Sequential)]
    public struct pam_conv
    {
        public IntPtr conv;
        public IntPtr appdata_ptr;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct pam_message
    {
        public int msg_style;
        [MarshalAs(UnmanagedType.LPStr)] public string msg;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct pam_response
    {
        [MarshalAs(UnmanagedType.LPStr)] public string resp;
        public int resp_retcode;
    }

    [LibraryImport("libpam", EntryPoint = "pam_start")]
    public static partial Status pam_start(
        [MarshalAs(UnmanagedType.LPStr)] string service,
        [MarshalAs(UnmanagedType.LPStr)] string? user,
        ref pam_conv pam_conversation,
        ref IntPtr pamh);

    [LibraryImport("libpam", EntryPoint = "pam_authenticate")]
    public static partial Status pam_authenticate(ref IntPtr pamh, Flags flags);

    [LibraryImport("libpam", EntryPoint = "pam_set_item")]
    public static partial Status pam_set_item(ref IntPtr pamh, ItemType item_type, [MarshalAs(UnmanagedType.LPStr)] string item);

    [LibraryImport("libpam", EntryPoint = "pam_error")]
    public static partial Status pam_error(ref IntPtr pamh, [MarshalAs(UnmanagedType.LPStr)] string format);

    [LibraryImport("libpam", EntryPoint = "pam_end")]
    public static partial Status pam_end(ref IntPtr pamh, Status status);
}
