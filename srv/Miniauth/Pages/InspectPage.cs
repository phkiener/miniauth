using System.Diagnostics.CodeAnalysis;
using Miniauth.Session;

namespace Miniauth.Pages;

public static class InspectPage
{
    public static string Render(UserSession? session)
    {
        return string.Format(
            Layout,
            Shared.Stylesheet,
            string.Format(
                Inspect,
                session is null
                    ? NotLoggedIn
                    : string.Format(LoggedIn, session.User, string.Join(", ", session.Scopes.DefaultIfEmpty("None")))));
    }

    [StringSyntax("html")]
    private const string Layout =
        """
        <!DOCTYPE html>
        <html lang="en">

        <head>
            <title>Miniauth</title>

            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width">

            {0}
        </head>

        <body>
           {1}
        </body>

        </html>
        """;

    [StringSyntax("html")]
    private const string Inspect =
        """
        <section class="card">
            <header><h2>Details</h2></header>
            {0}
        </form>
        """;

    [StringSyntax("html")]
    private const string LoggedIn =
        """
        <span>Logged in as {0}</span>
        <span>Scopes: {1}</span>

        <form method="get" action="/logout" style="display: contents">
            <button type="submit">Logout</button>
        </form>
        """;

    [StringSyntax("html")]
    private const string NotLoggedIn =
        """
        <span>You are not logged in.</span>

        <form method="get" action="/login" style="display: contents">
            <button type="submit">Login</button>
        </form>
        """;


}
