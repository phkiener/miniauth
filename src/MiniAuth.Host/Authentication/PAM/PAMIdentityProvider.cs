using System.Net.Sockets;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;

namespace MiniAuth.Host.Authentication.PAM;

public sealed class PAMIdentityProvider(IOptions<PAMOptions> options, ILogger<PAMIdentityProvider> logger) : IIdentityProvider
{
    public async Task<ClaimsPrincipal?> LoginAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        var path = Path.GetFullPath(options.Value.Socket);
        using var client = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);

        try
        {
            await client.ConnectAsync(new UnixDomainSocketEndPoint(path), cancellationToken);
            await client.SendAsync(Encoding.UTF8.GetBytes($"AUTH:{username}:{password}"), cancellationToken);

            var buffer = new byte[2048];
            var bytesRead = await client.ReceiveAsync(buffer, cancellationToken);

            var response = Encoding.UTF8.GetString(buffer[..bytesRead]);
            if (response.Split(':', StringSplitOptions.TrimEntries) is ["AUTH", var groupList])
            {
                var groups = groupList.Split(',', StringSplitOptions.TrimEntries);
                var identity = new ClaimsIdentity("PAM");
                identity.AddClaim(new Claim("name", username));

                foreach (var group in groups)
                {
                    identity.AddClaim(new Claim("group", group));
                }

                return new ClaimsPrincipal(identity);
            }

            return null;

        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to authenticate user {User} via PAM", username);
            return null;
        }
        finally
        {
            client.Close();
        }
    }
}
