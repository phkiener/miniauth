using System.Net.Sockets;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using MiniAuth.PAM.Proto;

namespace Miniauth.Server.Authentication.PAM;

public sealed class PAMIdentityProvider(IOptions<PAMOptions> options, ILogger<PAMIdentityProvider> logger) : IIdentityProvider
{
    public async Task<ClaimsPrincipal?> LoginAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            var path = Path.GetFullPath(options.Value.Socket);

            using var client = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);
            await client.ConnectAsync(new UnixDomainSocketEndPoint(path), cancellationToken);

            await using var stream = new NetworkStream(client);
            var request = new AuthenticateRequest(username, password);
            await request.WriteAsync(stream, cancellationToken);
            await stream.FlushAsync(cancellationToken);

            var response = await AuthenticateResponse.TryReadAsync(stream, cancellationToken);
            if (response is null || !response.Success)
            {
                return null;
            }

            var identity = new ClaimsIdentity("PAM");
            identity.AddClaim(new Claim("name", username));

            foreach (var group in response.Groups)
            {
                identity.AddClaim(new Claim("group", group));
            }

            return new ClaimsPrincipal(identity);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to authenticate user {User} via PAM", username);
            return null;
        }
    }
}
