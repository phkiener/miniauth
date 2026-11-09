using Microsoft.AspNetCore.Authentication;
using Miniauth.Server.Authentication.Identity;

namespace Miniauth.Server.Authentication;

public static class ServiceProviderConfiguration
{
    public static void AddAuthentication(this IServiceCollection services, IWebHostEnvironment environment)
    {
        services.AddAuthentication(ConfigureAuthentication);
        services.AddCascadingAuthenticationState();
        services.AddTransient<SessionCookieHandler>();

        if (environment.IsDevelopment())
        {
            services.AddScoped<IAuthenticationProvider, LocalLoginProvider>();
        }
        else
        {
            services.AddScoped<IAuthenticationProvider, ExternalAuthenticationProvider>();
        }
    }

    private static void ConfigureAuthentication(AuthenticationOptions options)
    {
        options.AddScheme<SessionCookieHandler>(SessionCookieHandler.Name, "Session Cookie");
        options.DefaultScheme = SessionCookieHandler.Name;
    }
}
