using Miniauth.Check;
using Miniauth.Check.Crypt;

if (args is not [var username] || username is "-h" or "--help")
{
    Console.Error.WriteLine("usage: miniauth-check $user; the password is read from stdin.");
    Console.Error.WriteLine("Exit codes:");
    Console.Error.WriteLine(" - 0: Credentials valid, stdout will contain all groups of the user (one per line)");
    Console.Error.WriteLine(" - 1: Credentials invalid");
    Console.Error.WriteLine(" - 127: Invocation error, e.g. unsupported arguments");
    Console.Error.WriteLine(" - 255: Program error or uncaught exception");

    return 127;
}

try
{
    var password = Console.ReadLine();
    var passwordChecker = new PasswordChecker(
        crypt: Libcrypt.Instance,
        shadowFile: Environment.GetEnvironmentVariable("SHADOWFILE") ?? "/etc/shadow",
        groupFile: Environment.GetEnvironmentVariable("GROUPFILE") ?? "/etc/group");

    var result = passwordChecker.Check(username, password ?? "");
    if (result is CheckResult.Success { Groups: var groups })
    {
        foreach (var group in groups)
        {
            Console.WriteLine(group);
        }

        return 0;
    }

    Console.Error.WriteLine("Invalid credentials");
    return 1;
}
catch (Exception e)
{
    Console.Error.WriteLine(e.Message);
    return 255;
}
