using System.Runtime.InteropServices;
using LibPAM.Types;

namespace LibPAM;

/// <summary>
/// Encapsulates a single PAM-based transaction.
/// </summary>
public sealed class Transaction : IDisposable
{
    private readonly ConversationDelegate? conversation;
    private Native.pam_conv conversationInfo;

    private bool disposed = false;
    private IntPtr handle = IntPtr.Zero;
    private Status lastStatus;

    /// <summary>
    /// Initialize a new PAM transaction.
    /// </summary>
    /// <param name="service">The service name to use.</param>
    /// <param name="conversation">A <see cref="ConversationDelegate"/> to use when handling messages.</param>
    /// <exception cref="InvalidOperationException">The transaction cannot be initialized.</exception>
    public Transaction(string service, ConversationDelegate? conversation = null)
    {
        this.conversation = conversation;

        // Make sure the pam_conv lives until after pam_end was called!
        conversationInfo = new Native.pam_conv
        {
            conv = Marshal.GetFunctionPointerForDelegate<Native.pam_conv_callback>(Converse),
            appdata_ptr = IntPtr.Zero
        };

        lastStatus = Native.pam_start(service, null, ref conversationInfo, ref handle);
        if (lastStatus is not Status.Success)
        {
            throw new InvalidOperationException($"Failed to initialize PAM transaction: {lastStatus}");
        }
    }

    /// <summary>
    /// Authenticate a user using <paramref name="username"/> and <paramref name="password"/>.
    /// </summary>
    /// <param name="username">The user to authenticate.</param>
    /// <param name="password">The password to supply as authentication token.</param>
    /// <returns><see langword="true"/> if the credentials are valid, <see langword="false"/> otherwise.</returns>
    public bool Authenticate(string username, string password)
    {
        ObjectDisposedException.ThrowIf(disposed, this);

        IntPtr usernamePointer = IntPtr.Zero;
        IntPtr passwordPointer = IntPtr.Zero;

        try
        {
            usernamePointer = Marshal.StringToHGlobalAuto(username);
            lastStatus = Native.pam_set_item(ref handle, ItemType.User, usernamePointer);
            if (lastStatus is not Status.Success)
            {
                return false;
            }

            passwordPointer = Marshal.StringToHGlobalAuto(password);
            lastStatus = Native.pam_set_item(ref handle, ItemType.AuthenticationToken, passwordPointer);
            if (lastStatus is not Status.Success)
            {
                return false;
            }

            lastStatus = Native.pam_authenticate(ref handle, Flags.Silent);
            return lastStatus is Status.Success;
        }
        finally
        {
            if (usernamePointer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(usernamePointer);
            }

            if (passwordPointer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(passwordPointer);
            }
        }
    }

    private Status Converse(
        int num_msg,
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] ref Native.pam_message[] msg,
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] ref Native.pam_response[] resp,
        ref IntPtr appdata_ptr)
    {
        if (conversation is not null)
        {
            var messages = msg.Select(static msg => new Message((MessageStyle)msg.msg_style, msg.msg)).ToArray();

            var result = conversation.Invoke(messages, out var responses);
            if (responses.Length != num_msg)
            {
                throw new InvalidOperationException($"Number of responses must match number of messages ({num_msg}), but was {responses.Length}");
            }

            resp = new Native.pam_response[num_msg];
            for (var i = 0; i < num_msg; ++i)
            {
                resp[i] = new Native.pam_response { resp = responses[i], resp_retcode = 0 };
            }

            return result;
        }

        return Status.ConversationError;
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
            lastStatus = Native.pam_end(ref handle, lastStatus);

            conversationInfo = new Native.pam_conv();
            handle = IntPtr.Zero;
        }

        disposed = true;
    }
}
