using System.Diagnostics.CodeAnalysis;
using Miniauth.Session;
using Npam;

namespace Miniauth.Authentication;

public sealed class PamAuthenticationProvider : IAuthenticationProvider
{
    public bool Authenticate(string username, string password, [NotNullWhen(true)] out UserSession? session)
    {
        var success = NpamUser.Authenticate("passwd", username, password);
        if (!success)
        {
            session = null;
            return false;
        }

        var groups = NpamUser.GetGroups(username);

        session = new UserSession(username, groups.Select(static g => g.GroupName));
        return true;
    }
}
