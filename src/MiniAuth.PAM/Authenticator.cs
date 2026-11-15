using LibPAM;
using LibPAM.Types;

namespace MiniAuth.PAM;

public sealed class Authenticator(string service)
{
    public bool HasAccess(string username, string password)
    {
        using var conversation = new Transaction(service, SendPassword);
        conversation.User = username;
        // conversation.Password = password;

        return conversation.Authenticate();
    }

    private static Status SendPassword(Message[] messages, ref string[] responses)
    {
        for (var i = 0; i < messages.Length; i++)
        {
            Console.Write(messages[i].Value);
            if (messages[i].Style is MessageStyle.Prompt or MessageStyle.PromptWithoutEcho)
            {
                Console.Write(" ");
                responses[i] = Console.ReadLine() ?? "";
            }
            else
            {
                Console.Write("\n");
                responses[i] = string.Empty;
            }
        }

        return Status.Success;
    }
}
