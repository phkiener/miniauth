using System.Net.Sockets;

namespace MiniAuth.PAM;

public delegate Task ClientHandler(Socket client, CancellationToken cancellationToken);

public sealed class Server : IDisposable
{
    private readonly Socket socket;
    private readonly ClientHandler handler;
    private bool running;

    private Server(string path, ClientHandler handler)
    {
        this.handler = handler;

        socket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.IP);
        socket.Bind(new UnixDomainSocketEndPoint(path));
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        if (running)
        {
            return;
        }

        running = true;
        socket.Listen(backlog: 10);
        Console.WriteLine($"Listening on {socket.LocalEndPoint}...");

        try
        {
            while (running && !cancellationToken.IsCancellationRequested)
            {
                var client = await socket.AcceptAsync(cancellationToken);
                _ = Task.Run(() => handler.Invoke(client, cancellationToken), cancellationToken);
            }
        }
        finally
        {
            Console.WriteLine("Shutting down...");
            running = false;
        }
    }

    public static Server Create(string path, ClientHandler handler)
    {
        var socketDirectory = Path.GetDirectoryName(path);
        if (socketDirectory is not (null or "") && !Directory.Exists(socketDirectory))
        {
            throw new DirectoryNotFoundException($"Path {socketDirectory} does not exist.");
        }

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        return new Server(Path.GetFullPath(path), handler);
    }

    public void Dispose()
    {
        socket.Dispose();
    }
}
