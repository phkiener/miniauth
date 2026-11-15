using LibPAM;

namespace MiniAuth.PAM;

public sealed class Authenticator(string service)
{
    public bool HasAccess(string username, string password)
    {
        using var conversation = new Transaction(service);
        conversation.User = username;
        conversation.Password = password;

        return conversation.Authenticate() && conversation.ValidateAccount();
    }
}
