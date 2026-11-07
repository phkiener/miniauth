using Miniauth.Check;
using Miniauth.Check.Files;

if (args is not [var username, var password])
{
    Console.Error.WriteLine("usage: miniauth-check $user $password");
    return 1;
}

var shadowEntry = ShadowFile.Read().FirstOrDefault(shadow => shadow.UserName == username);
if (!PasswordCheck.CheckPassword(shadowEntry?.PasswordHash, password))
{
    Console.Error.WriteLine("Invalid credentials");
    return 1;
}

var groups = GroupFile.Read().Where(g => g.Users.Contains(username));
foreach (var matchingGroup in groups)
{
    Console.WriteLine(matchingGroup.GroupName);
}

return 0;
