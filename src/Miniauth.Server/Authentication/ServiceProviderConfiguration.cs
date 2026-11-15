using Microsoft.AspNetCore.Authentication;
using Miniauth.Server.Authentication.Identity;
using Miniauth.Server.Authentication.Local;
using Miniauth.Server.Authentication.PAM;

namespace Miniauth.Server.Authentication;

public static class ServiceProviderConfiguration
{
    public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<IdentityOptions>()
            .BindConfiguration(string.Empty)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddAuthentication(ConfigureAuthentication);
        services.AddCascadingAuthenticationState();
        services.AddDataProtection();

        services.AddTransient<SessionCookieHandler>();

        switch (configuration.GetValue<string>("MINIAUTH_IDENTITY")?.ToUpperInvariant())
        {
            case "PAM":
                services.AddPAM();
                break;

            case "LOCAL":
                services.AddScoped<IIdentityProvider, LocalIdentityProvider>();
                break;

            default:
                throw new InvalidOperationException("No identity provider specified in MINIAUTH_IDENTITY.");
        }
    }

    private static void ConfigureAuthentication(AuthenticationOptions options)
    {
        options.AddScheme<SessionCookieHandler>(SessionCookieHandler.Name, "Session Cookie");
        options.DefaultScheme = SessionCookieHandler.Name;
    }
}
