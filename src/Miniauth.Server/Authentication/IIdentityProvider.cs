using System.Security.Claims;

namespace Miniauth.Server.Authentication;

public interface IIdentityProvider
{
    public Task<ClaimsPrincipal?> LoginAsync(string username, string password, CancellationToken cancellationToken = default);
}
