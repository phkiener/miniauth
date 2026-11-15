using System.Runtime.InteropServices;
using LibPAM.Types;

namespace LibPAM;

/// <summary>
/// Encapsulates a single PAM-based transaction.
/// </summary>
public sealed class Transaction : IDisposable
{
    private readonly ConversationDelegate? conversationDelegate;

    private bool disposed = false;
    private IntPtr conversation;
    private IntPtr handle = IntPtr.Zero;

    /// <summary>
    /// Initialize a new PAM transaction.
    /// </summary>
    /// <param name="service">The service name to use.</param>
    /// <param name="conversationDelegate">The <see cref="conversationDelegate"/> to use for handling the callbacks from PAM.</param>
    /// <exception cref="InvalidOperationException">The transaction cannot be initialized.</exception>
    public Transaction(string service, ConversationDelegate? conversationDelegate = null)
    {
        this.conversationDelegate = conversationDelegate;
        conversation = Marshal.AllocHGlobal(Marshal.SizeOf<Native.pam_conv>());
        Marshal.StructureToPtr(new Native.pam_conv { conv = Marshal.GetFunctionPointerForDelegate<Native.pam_conv_callback>(Converse), appdata_ptr = IntPtr.Zero }, conversation, false);

        Status = Native.pam_start(service, null, conversation, ref handle);
        if (Status is not Status.Success)
        {
            throw new InvalidOperationException($"Failed to initialize PAM transaction: {Status}");
        }
    }

    /// <summary>
    /// Status of the last operation.
    /// </summary>
    public Status Status { get; private set; }

    /// <summary>
    /// The user whom the transaction is about
    /// </summary>
    public string? Service
    {
        get => GetStringItemOrThrow(ItemType.Service);
        set => SetStringItemOrThrow(ItemType.Service, value);
    }

    /// <summary>
    /// The user whom the transaction is about
    /// </summary>
    public string? User
    {
        get => GetStringItemOrThrow(ItemType.User);
        set => SetStringItemOrThrow(ItemType.User, value);
    }

    /// <summary>
    /// The password to use for silent authentication.
    /// </summary>
    public string? Password
    {
        get => GetStringItemOrThrow(ItemType.AuthenticationToken);
        set => SetStringItemOrThrow(ItemType.AuthenticationToken, value);
    }

    /// <summary>
    /// Authenticate the user.
    /// </summary>
    /// <param name="flags">The <see cref="Flags"/> to use when authenticating.</param>
    /// <returns><see langword="true"/> if authentication was successful, <see langword="false"/> otherwise.</returns>
    public bool Authenticate(Flags flags = 0)
    {
        ObjectDisposedException.ThrowIf(disposed, this);

        Status = Native.pam_authenticate(handle, flags);
        return Status == Status.Success;
    }

    /// <summary>
    /// Validate the user's account (not expired, no access restrictions, ...).
    /// </summary>
    /// <param name="flags">The <see cref="Flags"/> to use when validating.</param>
    /// <returns><see langword="true"/> if the account is valid, <see langword="false"/> otherwise.</returns>
    public bool ValidateAccount(Flags flags = 0)
    {
        ObjectDisposedException.ThrowIf(disposed, this);

        Status = Native.pam_acct_mgmt(handle, flags);
        return Status is Status.Success;
    }

    private string? GetStringItemOrThrow(ItemType itemType)
    {
        ObjectDisposedException.ThrowIf(disposed, this);

        Status = Native.pam_get_item(handle, itemType, out var unmanagedString);
        if (Status is not Status.Success)
        {
            throw new InvalidOperationException($"Failed to get item of type {itemType}: {Status}");
        }

        return Marshal.PtrToStringAnsi(unmanagedString);
    }

    private void SetStringItemOrThrow(ItemType itemType, string? value)
    {
        ObjectDisposedException.ThrowIf(disposed, this);

        var unmanagedString = Marshal.StringToHGlobalAnsi(value);
        Status = Native.pam_set_item(handle, itemType, unmanagedString);
        if (Status is not Status.Success)
        {
            throw new InvalidOperationException($"Failed to set item of type {itemType}: {Status}");
        }

        Marshal.FreeHGlobal(unmanagedString);
    }

    private Status Converse(int num_msg, ref IntPtr msg, ref IntPtr resp, IntPtr appdata)
    {
        if (conversationDelegate is null)
        {
            resp = IntPtr.Zero;
            return Status.ConversationError;
        }

        var messages = new Message[num_msg];
        for (var i = 0; i < num_msg; i++)
        {
            var rawMessage = Marshal.PtrToStructure<Native.pam_message>(msg + i);
            messages[i] = new Message(rawMessage.msg_style, rawMessage.msg);
        }

        var responses = new string[num_msg];
        var result = conversationDelegate.Invoke(messages, ref responses);

        // Do not free this memory. The caller of the callback (i.e. not us) is responsible to free the responses.
        resp = Marshal.AllocHGlobal(Marshal.SizeOf<Native.pam_response>() * num_msg);
        for (var i = 0; i < num_msg; i++)
        {
            var offset = Marshal.SizeOf<Native.pam_response>() * i;
            Marshal.StructureToPtr(new Native.pam_response { resp = responses[i], resp_retcode = 0 }, resp + offset, false);
        }

        return result;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        if (handle != IntPtr.Zero)
        {
            Status = Native.pam_end(handle, Status);
            handle = IntPtr.Zero;
        }

        if (conversation != IntPtr.Zero)
        {
            Marshal.DestroyStructure<Native.pam_conv>(conversation);
            Marshal.FreeHGlobal(conversation);

            conversation = IntPtr.Zero;
        }

        disposed = true;
    }
}
