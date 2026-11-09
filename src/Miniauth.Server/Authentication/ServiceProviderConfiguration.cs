using Microsoft.AspNetCore.Authentication;
using Miniauth.Server.Authentication.Identity;

namespace Miniauth.Server.Authentication;

public static class ServiceProviderConfiguration
{
    public static void AddAuthentication(this IServiceCollection services, IWebHostEnvironment environment)
    {
        services.AddAuthentication(ConfigureAuthentication);
        services.AddCascadingAuthenticationState();
        services.AddDataProtection();

        services.AddTransient<SessionCookieHandler>();

        if (environment.IsDevelopment())
        {
            services.AddScoped<IIdentityProvider, LocalIdentityProvider>();
        }
        else
        {
            services.AddScoped<IIdentityProvider, ExternalIdentityProvider>();
        }
    }

    private static void ConfigureAuthentication(AuthenticationOptions options)
    {
        options.AddScheme<SessionCookieHandler>(SessionCookieHandler.Name, "Session Cookie");
        options.DefaultScheme = SessionCookieHandler.Name;
    }
}
