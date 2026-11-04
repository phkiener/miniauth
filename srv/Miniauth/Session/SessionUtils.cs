using System.Text;
using Microsoft.AspNetCore.DataProtection;

namespace Miniauth.Session;

public static class SessionUtils
{
    private const string CookieName = "_auth";

    private static readonly CookieOptions CookieOptions = new()
    {
        HttpOnly = true,
        Secure = true,
        MaxAge = TimeSpan.FromDays(7)
    };

    public static UserSession? ReadSession(this HttpContext context)
    {
        if (!context.Request.Cookies.TryGetValue(CookieName, out var cookie))
        {
            return null;
        }

        if (cookie is null or "")
        {
            return null;
        }

        try
        {
            return Unprotect(cookie, context.RequestServices);
        }
        catch
        {
            return null;
        }
    }

    public static void WriteSession(this HttpContext context, UserSession session)
    {
        var protectedSession = Protect(session, context.RequestServices);
        context.Response.Cookies.Append(CookieName, protectedSession, CookieOptions);
    }

    private static UserSession? Unprotect(string cookie, IServiceProvider serviceProvider)
    {
        var dataProtector = serviceProvider.GetRequiredService<IDataProtector>();
        var unprotectedBytes = dataProtector.Unprotect(Convert.FromBase64String(cookie));
        var serializedSession = Encoding.UTF8.GetString(unprotectedBytes);

        return UserSession.TryParse(serializedSession, out var session) ? session : null;
    }

    private static string Protect(UserSession session, IServiceProvider serviceProvider)
    {
        var dataProtector = serviceProvider.GetRequiredService<IDataProtector>();
        var serializedSession = Encoding.UTF8.GetBytes(session.Serialize());
        var protectedBytes = dataProtector.Protect(serializedSession);

        return Convert.ToBase64String(protectedBytes);
    }
}
