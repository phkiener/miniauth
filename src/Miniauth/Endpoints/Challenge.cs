using Miniauth.Pages;
using Miniauth.Session;

namespace Miniauth.Endpoints;

public static class Challenge
{
    public static Task HandleAsync(HttpContext context)
    {
        var requiredScope = context.Request.Query["scope"];
        var session = context.ReadSession();

        if (session is null)
        {
            var originalMethod = context.Request.Headers["X-Forwarded-Method"];
            var originalUri = context.Request.Headers["X-Forwarded-Uri"];

            context.Response.Redirect($"/login?method={originalMethod}&uri={originalUri}");
            return Task.CompletedTask;
        }

        if (requiredScope.Count is 0)
        {
            context.Response.StatusCode = StatusCodes.Status204NoContent;
            return Task.CompletedTask;
        }

        if (requiredScope.All(s => session.Scopes.Contains(s)))
        {
            context.Response.StatusCode = StatusCodes.Status204NoContent;
            return Task.CompletedTask;
        }

        context.Response.StatusCode = StatusCodes.Status403Forbidden;

        var accessDeniedPage = AccessDeniedPage.Render();

        context.Response.StatusCode = StatusCodes.Status200OK;
        context.Response.ContentType = "text/html; charset=utf-8";
        return context.Response.WriteAsync(accessDeniedPage);
    }
}
