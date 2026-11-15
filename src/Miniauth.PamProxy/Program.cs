using LibC;
using LibPAM;
using LibPAM.Types;

if (args is not [var service, var username])
{
    return 1;
}

try
{
    using var conversation = new Transaction(service, Handle);
    conversation.User = username;
    var access = conversation.Authenticate();
    access &= conversation.ValidateAccount();
    Console.WriteLine($"Has access: {access}");

    var groups = Groups.EnumerateGroups(username).ToList();
    Console.WriteLine($"Groups are: {string.Join(", ", groups)}");

    return 0;
}
catch (Exception e)
{
    Console.WriteLine(e);
    return 1;
}

static Status Handle(Message[] messages, ref string[] responses)
{
    for (var i = 0; i < messages.Length; ++i)
    {
        Console.Write(messages[i].Value);
        if (messages[i].Style is MessageStyle.Prompt or MessageStyle.PromptWithoutEcho)
        {
            Console.Write(" ");
            responses[i] = Console.ReadLine() ?? string.Empty;
        }
        else
        {
            Console.WriteLine();
            responses[i] = "";
        }
    }

    return Status.Success;
}
