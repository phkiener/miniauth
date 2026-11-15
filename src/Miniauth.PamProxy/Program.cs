using System.Net.Sockets;
using LibC;
using LibPAM;
using Miniauth.PamProxy.Proto;

if (args is not [var service, var socket])
{
    Console.Error.WriteLine("Usage: miniauth-pam <service> <socket-file>");
    return 1;
}

var cancellationTokenSource = new CancellationTokenSource();
Console.CancelKeyPress += (_, _) => cancellationTokenSource.Cancel();

var socketDirectory = Path.GetDirectoryName(socket);
if (socketDirectory is not (null or "") && !Directory.Exists(socketDirectory))
{
    Console.Error.WriteLine($"Directory '{socketDirectory}' does not exist");
    return 1;
}

if (File.Exists(socket))
{
    File.Delete(socket);
}

try
{
    using var listener = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.IP);
    listener.Bind(new UnixDomainSocketEndPoint(socket));
    listener.Listen(backlog: 10);

    while (!cancellationTokenSource.IsCancellationRequested)
    {
        var client = await listener.AcceptAsync(cancellationTokenSource.Token);
        _ = Task.Run(() => HandleClient(client, service, cancellationTokenSource.Token), cancellationTokenSource.Token);
    }
}
finally
{
    if (File.Exists(socket))
    {
        File.Delete(socket);
    }
}

return 0;

static async Task HandleClient(Socket client, string service, CancellationToken cancellationToken)
{
    try
    {
        await using var networkStream = new NetworkStream(client, true);

        var request = await AuthenticateRequest.TryReadAsync(networkStream, cancellationToken);
        if (request is null)
        {
            return; // protocol error, just deny.
        }

        using var conversation = new Transaction(service);
        conversation.User = request.User;
        conversation.Password = request.Password;

        var isAuthenticated = conversation.Authenticate();
        var groups = isAuthenticated ? Groups.EnumerateGroups(request.User).ToArray() : [];

        var response = new AuthenticateResponse(isAuthenticated, groups);
        await response.WriteAsync(networkStream, cancellationToken);
    }
    finally
    {
        client.Close();
    }
}
