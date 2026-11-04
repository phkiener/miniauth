using Miniauth.Session;

namespace Miniauth.Endpoints;

public static class Inspect
{
    public static Task HandleAsync(HttpContext context)
    {
        var session = context.ReadSession();

        context.Response.StatusCode = StatusCodes.Status200OK;
        return context.Response.WriteAsJsonAsync(session);
    }
}
