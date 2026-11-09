using Miniauth.Check;
using Miniauth.Check.Files;

if (args is not [var username] || username is "-h" or "--help")
{
    Console.Error.WriteLine("usage: miniauth-check $user; the password is read from stdin.");
    return 1;
}

try
{
    var password = Console.ReadLine();
    var passwordHash = ShadowFile.FindPasswordHash(username);

    if (!Crypt.CheckPassword(passwordHash ?? "", password ?? ""))
    {
        Console.Error.WriteLine("Invalid credentials");
        return 1;
    }

    foreach (var group in GroupFile.FindGroups(username))
    {
        Console.WriteLine(group);
    }

    return 0;
}
catch (Exception e)
{
    Console.Error.WriteLine(e.Message);
    return 255;
}
