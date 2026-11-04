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
            // TODO: Store X-Forwarded-Method & X-Forwarded-Uri
            context.Response.Redirect("/login");
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
        return Task.CompletedTask;
    }
}
