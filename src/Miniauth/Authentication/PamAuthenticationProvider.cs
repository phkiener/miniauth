using System.Diagnostics.CodeAnalysis;
using Miniauth.Session;
using Npam;

namespace Miniauth.Authentication;

public sealed class PamAuthenticationProvider(IConfiguration configuration, ILogger<PamAuthenticationProvider> logger) : IAuthenticationProvider
{
    public bool Authenticate(string username, string password, [NotNullWhen(true)] out UserSession? session)
    {
        var service = configuration.GetValue<string>("PAM_SERVICENAME") ?? "passwd";

        logger.LogInformation("Checking access for {Name} with {Service}", username, service);
        var success = NpamUser.Authenticate(service, username, password);
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
