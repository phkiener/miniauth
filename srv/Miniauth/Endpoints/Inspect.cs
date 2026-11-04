using Miniauth.Pages;
using Miniauth.Session;

namespace Miniauth.Endpoints;

public static class Inspect
{
    public static Task HandleAsync(HttpContext context)
    {
        var session = context.ReadSession();
        var loginPage = InspectPage.Render(session);

        return context.Response.WriteAsync(loginPage);
    }
}
