using LibPAM;
using LibPAM.Types;

namespace MiniAuth.PAM;

public sealed class Authenticator(string service)
{
    public bool HasAccess(string username, string password)
    {
        using var conversation = new Transaction(service, (messages, ref strings) => SendPassword(password, messages, ref strings));
        conversation.User = username;
        // conversation.Password = password;

        return conversation.Authenticate();
    }

    private static Status SendPassword(string password, Message[] messages, ref string[] responses)
    {
        if (messages is [{ Value: "Password:" }])
        {
            responses[0] = password;
            return Status.Success;
        }

        return Status.ConversationError;
    }
}
