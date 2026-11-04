using System.Diagnostics.CodeAnalysis;

namespace Miniauth.Pages;

public static class AccessDeniedPage
{
    public static string Render()
    {
        return string.Format(
            Layout,
            Shared.Stylesheet,
            Content);
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
    private const string Content =
        """
        <section class="card">
            <header><h2>Access denied</h2></header>
            <span>You do not have access to this page.</span>
        </section>
        """;


}
