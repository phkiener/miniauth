using System.Net.Sockets;
using LibC;
using MiniAuth.PAM;
using MiniAuth.PAM.Proto;

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
        await using var networkStream = new NetworkStream(client, true);

        var request = await AuthenticateRequest.TryReadAsync(networkStream, cancellationToken);
        if (request is null)
        {
            return; // protocol error, just abort.
        }

        var isAuthenticated = authenticator.HasAccess(request.User, request.Password);
        var groups = isAuthenticated ? Groups.EnumerateGroups(request.User).ToArray() : [];

        var response = new AuthenticateResponse(isAuthenticated, groups);
        await response.WriteAsync(networkStream, cancellationToken);
    }
    finally
    {
        client.Close();
    }
}
