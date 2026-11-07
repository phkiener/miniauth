using Miniauth.Session;

namespace Miniauth.Authentication;

public interface IAuthenticationProvider
{
    public Task<UserSession?> AuthenticateAsync(string username, string password, CancellationToken cancellationToken = default);
}
