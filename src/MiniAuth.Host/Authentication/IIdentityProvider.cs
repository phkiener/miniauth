using System.Security.Claims;

namespace MiniAuth.Host.Authentication;

public interface IIdentityProvider
{
    public Task<ClaimsPrincipal?> LoginAsync(string username, string password, CancellationToken cancellationToken = default);
}
