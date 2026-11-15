namespace MiniAuth.PAM.Proto.Internal;

public interface IMessage<T>
{
    static abstract Task<T?> TryReadAsync(Stream stream, CancellationToken cancellationToken = default);
    Task WriteAsync(Stream stream, CancellationToken cancellationToken = default);
}
