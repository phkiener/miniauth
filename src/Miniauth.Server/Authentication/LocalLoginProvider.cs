using System.Security.Claims;

namespace Miniauth.Server.Authentication;

public sealed class LocalLoginProvider : IAuthenticationProvider
{
    public Task<ClaimsPrincipal?> AuthenticateAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        if (username is "AzureDiamond" && password is "hunter2")
        {
            var identity = new ClaimsIdentity("local");
            identity.AddClaim(new Claim("name", username));
            identity.AddClaim(new Claim("group", "IRC user"));
            identity.AddClaim(new Claim("group", "bash.org legend"));
            return Task.FromResult<ClaimsPrincipal?>(new ClaimsPrincipal(identity));
        }

        return Task.FromResult<ClaimsPrincipal?>(null);
    }
}
