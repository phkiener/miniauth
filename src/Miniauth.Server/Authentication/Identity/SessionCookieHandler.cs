using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace Miniauth.Server.Authentication.Identity;

public sealed class SessionCookieHandler(IOptions<MiniauthOptions> options, IDataProtectionProvider dataProtectionProvider) : IAuthenticationSignInHandler
{
    private readonly SecureDataFormat<AuthenticationTicket> ticketFormat = new(
        serializer: new AuthenticationTicketSerializer(),
        protector: dataProtectionProvider.CreateProtector(Name));

    private HttpContext? httpContext;

    public const string Name = "session-cookie";
    public const string CookieName = ".miniauth";

    public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
    {
        if (scheme.Name is Name)
        {
            httpContext = context;
        }

        return Task.CompletedTask;
    }

    public Task<AuthenticateResult> AuthenticateAsync()
    {
        var cookieValue = httpContext?.Request.Cookies[CookieName];
        if (cookieValue is null)
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var result = ticketFormat.Unprotect(cookieValue);
        if (result?.AuthenticationScheme is not Name)
        {
            return Task.FromResult(AuthenticateResult.Fail("Cannot read session cookie"));
        }

        return Task.FromResult(AuthenticateResult.Success(result));
    }

    public Task ChallengeAsync(AuthenticationProperties? properties)
    {
        if (properties?.GetParameter<bool>("allow-redirect") is true)
        {
            var queryParameters = new Dictionary<string, string?>
            {
                ["method"] = properties.GetParameter<string>("method"),
                ["proto"] = properties.GetParameter<string>("proto"),
                ["host"] = properties.GetParameter<string>("host"),
                ["uri"] = properties.GetParameter<string>("uri"),
            };

            var redirectTarget = QueryHelpers.AddQueryString($"{options.Value.LoginHost}/login", queryParameters);

            httpContext?.Response.Redirect(redirectTarget);
            return Task.CompletedTask;
        }

        httpContext?.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    }

    public Task ForbidAsync(AuthenticationProperties? properties)
    {
        if (properties?.GetParameter<bool>("allow-redirect") is true)
        {
            var queryParameters = new Dictionary<string, string?>
            {
                ["method"] = properties.GetParameter<string>("method"),
                ["proto"] = properties.GetParameter<string>("proto"),
                ["host"] = properties.GetParameter<string>("host"),
                ["uri"] = properties.GetParameter<string>("uri"),
            };

            var redirectTarget = QueryHelpers.AddQueryString($"{options.Value.LoginHost}/forbidden", queryParameters);

            httpContext?.Response.Redirect(redirectTarget);
            return Task.CompletedTask;
        }

        httpContext?.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    }

    public Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties? properties)
    {
        var cookieValue = ticketFormat.Protect(new AuthenticationTicket(user, Name));
        var cookieOptions = new CookieOptions
        {
            Secure = true,
            HttpOnly = true,
            IsEssential = true,
            SameSite = SameSiteMode.Lax,

            Domain = options.Value.Domain,
            MaxAge = TimeSpan.FromDays(options.Value.SessionLifetimeDays)
        };

        httpContext?.Response.Cookies.Append(CookieName, cookieValue, cookieOptions);

        return Task.CompletedTask;
    }

    public Task SignOutAsync(AuthenticationProperties? properties)
    {
        httpContext?.Response.Cookies.Delete(CookieName, new CookieOptions { Domain = options.Value.Domain });
        return Task.CompletedTask;
    }
}
