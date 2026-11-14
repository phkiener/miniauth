using LibPAM;

if (args is not [var service, var username, var password])
{
    return 1;
}

try
{
    using var conversation = new Transaction(service);
    var access = conversation.Authenticate(username, password);

    Console.WriteLine($"Has access: {access}");
    return 0;
}
catch (Exception e)
{
    Console.WriteLine(e);
    return 1;
}
