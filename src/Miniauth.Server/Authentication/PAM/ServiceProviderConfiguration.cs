namespace Miniauth.Server.Authentication.PAM;

public static class ServiceProviderConfiguration
{
    public static void AddPAM(this IServiceCollection services)
    {
        services.AddOptions<PAMOptions>()
            .BindConfiguration(string.Empty)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped<IIdentityProvider, PAMIdentityProvider>();
    }
}
