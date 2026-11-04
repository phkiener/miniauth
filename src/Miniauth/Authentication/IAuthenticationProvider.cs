using System.Diagnostics.CodeAnalysis;
using Miniauth.Session;

namespace Miniauth.Authentication;

public interface IAuthenticationProvider
{
    public bool Authenticate(string username, string password, [NotNullWhen(true)] out UserSession? session);
}
