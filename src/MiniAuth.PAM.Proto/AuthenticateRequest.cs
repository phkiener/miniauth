using System.Net;
using System.Text;

namespace MiniAuth.PAM.Proto;

public sealed record AuthenticateRequest(string User, string Password) : IMessage<AuthenticateRequest>
{
    public static async Task<AuthenticateRequest?> TryReadAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        if (!stream.CanRead)
        {
            throw new ArgumentException("Stream must be readable", nameof(stream));
        }

        var header = new byte[4];
        await stream.ReadExactlyAsync(header, cancellationToken);
        if (!header.SequenceEqual("auth"u8))
        {
            return null;
        }

        var size = new byte[4];
        await stream.ReadExactlyAsync(size, cancellationToken);
        var messageSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(size));

        var message = new byte[messageSize];
        await stream.ReadExactlyAsync(message, cancellationToken);
        var text = Encoding.UTF8.GetString(message);

        return text.Split(':') is [var username, var password] ? new AuthenticateRequest(username, password) : null;
    }

    public async Task WriteAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        var size = new byte[4];
        var messageBytes = Encoding.UTF8.GetBytes($"{User}:{Password}");
        BitConverter.TryWriteBytes(size, IPAddress.HostToNetworkOrder(messageBytes.Length));

        await stream.WriteAsync("auth"u8.ToArray(), cancellationToken);
        await stream.WriteAsync(size, cancellationToken);
        await stream.WriteAsync(messageBytes, cancellationToken);
    }

}
