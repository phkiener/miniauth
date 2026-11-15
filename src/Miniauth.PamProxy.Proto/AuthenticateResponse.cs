using System.Text.Json;
using System.Text.Json.Nodes;
using Miniauth.PamProxy.Proto.Internal;

namespace Miniauth.PamProxy.Proto;

public sealed record AuthenticateResponse(bool Success, string[] Groups) : IMessage<AuthenticateResponse>
{
    private const string OperationName = "authenticate";

    public static async Task<AuthenticateResponse?> TryReadAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        if (!stream.CanRead)
        {
            throw new ArgumentException("Stream must be readable", nameof(stream));
        }

        var node = await JsonNode.ParseAsync(stream, cancellationToken: cancellationToken);
        var command = node.Deserialize<Response>(SerializerContext.Default.Response);

        return command?.Operation is not OperationName ? null : new AuthenticateResponse(command.Success, command.Groups);
    }

    public async Task WriteAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        var command = new Response(OperationName, Success, Groups);
        await JsonSerializer.SerializeAsync(stream, command, SerializerContext.Default.Response, cancellationToken: cancellationToken);
    }

    internal sealed record Response(string Operation, bool Success, string[] Groups);
}
