using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using Miniauth.Authentication;
using Miniauth.Pages;
using Miniauth.Session;

namespace Miniauth.Endpoints;

public static class Login
{
    public static Task HandleAsync(HttpContext context)
    {
        var session = context.ReadSession();
        if (session is not null)
        {
            context.Response.StatusCode = StatusCodes.Status204NoContent;
            return Task.CompletedTask;
        }

        var wrongCredentials = context.Request.Query.ContainsKey("error");
        var loginPage = LoginPage.Render(showCredentialError: wrongCredentials);

        return context.Response.WriteAsync(loginPage);
    }

    public static async Task HandlePostAsync(HttpContext context)
    {
        var form = await context.Request.ReadFormAsync();
        var username = form["username"].ToString();
        var password = form["password"].ToString();

        var authenticationProvider = context.RequestServices.GetRequiredService<IAuthenticationProvider>();
        if (!authenticationProvider.Authenticate(username, password, out var session))
        {
            var redirectTarget = QueryHelpers.AddQueryString(context.Request.GetEncodedPathAndQuery(), "error", "true");
            context.Response.Redirect(redirectTarget);

            return;
        }

        context.WriteSession(session);

        var originalMethod = context.Request.Query["method"].ToString();
        var originalUri = Uri.UnescapeDataString(context.Request.Query["uri"].ToString());

        if (originalMethod.Equals("get", StringComparison.OrdinalIgnoreCase))
        {
            context.Response.Redirect(originalUri);
            return;
        }

        context.Response.Redirect("/");
    }
}
