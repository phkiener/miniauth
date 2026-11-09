using System.Security.Claims;

namespace Miniauth.Server.Authentication;

public interface IAuthenticationProvider
{
    public Task<ClaimsPrincipal?> AuthenticateAsync(string username, string password, CancellationToken cancellationToken = default);
}
