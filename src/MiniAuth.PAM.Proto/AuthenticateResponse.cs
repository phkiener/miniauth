using System.Net;
using System.Text;

namespace MiniAuth.PAM.Proto;

public sealed record AuthenticateResponse(bool Success, string[] Groups) : IMessage<AuthenticateResponse>
{
    public static async Task<AuthenticateResponse?> TryReadAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        if (!stream.CanRead)
        {
            throw new ArgumentException("Stream must be readable", nameof(stream));
        }

        var header = new byte[4];
        await stream.ReadExactlyAsync(header, cancellationToken);
        if (!header.SequenceEqual("resp"u8))
        {
            return null;
        }

        var size = new byte[4];
        await stream.ReadExactlyAsync(size, cancellationToken);
        var messageSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(size));

        var message = new byte[messageSize];
        await stream.ReadExactlyAsync(message, cancellationToken);
        var text = Encoding.UTF8.GetString(message);

        if (text.Split(':') is not [var success, var groups] || !bool.TryParse(success, out var isSuccess))
        {
            return null;
        }

        return new AuthenticateResponse(isSuccess, groups.Split(','));
    }

    public async Task WriteAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        var size = new byte[4];
        var messageBytes = Encoding.UTF8.GetBytes($"{Success}:{string.Join(",", Groups)}");
        BitConverter.TryWriteBytes(size, IPAddress.HostToNetworkOrder(messageBytes.Length));

        await stream.WriteAsync("resp"u8.ToArray(), cancellationToken);
        await stream.WriteAsync(size, cancellationToken);
        await stream.WriteAsync(messageBytes, cancellationToken);
    }
}
