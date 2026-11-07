using Miniauth.Pages;
using Miniauth.Session;

namespace Miniauth.Endpoints;

public static class Inspect
{
    public static Task HandleAsync(HttpContext context)
    {
        var session = context.ReadSession();
        var loginPage = InspectPage.Render(session);

        context.Response.StatusCode = StatusCodes.Status200OK;
        context.Response.ContentType = "text/html; charset=utf-8";
        return context.Response.WriteAsync(loginPage);
    }
}
