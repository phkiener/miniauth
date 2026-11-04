using Miniauth.Session;

namespace Miniauth.Endpoints;

public static class Logout
{
    public static Task HandleAsync(HttpContext context)
    {
        context.ClearSession();
        context.Response.Redirect("/");

        return Task.CompletedTask;
    }
}
