using Microsoft.AspNetCore.Http.HttpResults;
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

        var result = new RazorComponentResult<LoginPage>();
        return result.ExecuteAsync(context);
    }

    public static async Task HandlePostAsync(HttpContext context)
    {
        var form = await context.Request.ReadFormAsync();
        var username = form["username"];
        var password = form["password"];

        // TODO: login

        var session = new UserSession(username!, ["admin"]);
        context.WriteSession(session);
    }
}
