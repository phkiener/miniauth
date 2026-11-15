using System.Net.Sockets;
using System.Text;
using LibC;
using MiniAuth.PAM;

if (args is not [var service, var socket])
{
    Console.Error.WriteLine("Usage: miniauth-pam <service> <socket-file>");
    return 1;
}

var cancellationTokenSource = new CancellationTokenSource();
Console.CancelKeyPress += (_, _) => cancellationTokenSource.Cancel();

var authenticator = new Authenticator(service);
using var server = Server.Create(socket, HandleClient);
await server.RunAsync(cancellationTokenSource.Token);

return 0;

async Task HandleClient(Socket client, CancellationToken cancellationToken)
{
    try
    {
        var buffer = new byte[2048];

        var bytesRead = await client.ReceiveAsync(buffer, cancellationToken);
        var command = Encoding.UTF8.GetString(buffer[..bytesRead]);

        if (command.Split(':') is ["AUTH", var username, var password])
        {
            var isAuthenticated = authenticator.HasAccess(username, password);
            var groups = isAuthenticated ? Groups.EnumerateGroups(username).ToArray() : [];

            var response = isAuthenticated ? $"AUTH:{string.Join(",", groups)}" : "FAIL";
            await client.SendAsync(Encoding.UTF8.GetBytes(response), cancellationToken);
        }
        else
        {
            await client.SendAsync("ERROR"u8.ToArray(), cancellationToken);
        }

        client.Close();
    }
    finally
    {
        client.Dispose();
    }
}
