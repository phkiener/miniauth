using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;

namespace Miniauth.Server.Authentication.Identity;

public sealed class SessionCookieHandler(IOptions<SessionCookieOptions> options, IDataProtectionProvider dataProtectionProvider) : IAuthenticationSignInHandler
{
    public const string Name = "session-cookie";

    private HttpContext? httpContext;

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
        var cookieName = options.Value.CookieName;
        var cookieValue = httpContext?.Request.Cookies[cookieName];
        if (cookieValue is null)
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var dataProtector = dataProtectionProvider.CreateProtector(Name);
        var format = new SecureDataFormat<AuthenticationTicket>(options.Value.TicketSerializer, dataProtector);

        var result = format.Unprotect(cookieValue);
        if (result?.AuthenticationScheme is not Name)
        {
            return Task.FromResult(AuthenticateResult.Fail("Cannot read session cookie"));
        }

        return Task.FromResult(AuthenticateResult.Success(result));
    }

    public Task ChallengeAsync(AuthenticationProperties? properties)
    {
        httpContext?.Response.StatusCode = StatusCodes.Status401Unauthorized;

        return Task.CompletedTask;
    }

    public Task ForbidAsync(AuthenticationProperties? properties)
    {
        httpContext?.Response.StatusCode = StatusCodes.Status403Forbidden;

        return Task.CompletedTask;
    }

    public Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties? properties)
    {
        var dataProtector = dataProtectionProvider.CreateProtector(Name);
        var format = new SecureDataFormat<AuthenticationTicket>(options.Value.TicketSerializer, dataProtector);

        var cookieValue = format.Protect(new AuthenticationTicket(user, Name));
        var cookieOptions = new CookieOptions()
        {
            Secure = true,
            HttpOnly = true,
            IsEssential = true,
            SameSite = SameSiteMode.Lax,

            Domain = options.Value.CookieDomain,
            MaxAge = options.Value.CookieMaxAge
        };

        httpContext?.Response.Cookies.Append(options.Value.CookieName, cookieValue, cookieOptions);

        return Task.CompletedTask;
    }

    public Task SignOutAsync(AuthenticationProperties? properties)
    {
        httpContext?.Response.Cookies.Delete(options.Value.CookieName);
        return Task.CompletedTask;
    }
}
