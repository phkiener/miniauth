using System.Text.Json;
using System.Text.Json.Nodes;
using Miniauth.PamProxy.Proto.Internal;

namespace Miniauth.PamProxy.Proto;

public sealed record AuthenticateRequest(string User, string Password) : IMessage<AuthenticateRequest>
{
    private const string OperationName = "authenticate";

    public static async Task<AuthenticateRequest?> TryReadAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        if (!stream.CanRead)
        {
            throw new ArgumentException("Stream must be readable", nameof(stream));
        }

        var node = await JsonNode.ParseAsync(stream, cancellationToken: cancellationToken);
        var command = node.Deserialize<Command>(SerializerContext.Default.Command);

        return command?.Operation is not OperationName ? null : new AuthenticateRequest(command.User, command.Password);
    }

    public async Task WriteAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        var command = new Command(OperationName, User, Password);
        await JsonSerializer.SerializeAsync(stream, command, SerializerContext.Default.Command, cancellationToken: cancellationToken);
    }

    internal sealed record Command(string Operation, string User, string Password);

}
