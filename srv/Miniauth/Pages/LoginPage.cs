using System.Diagnostics.CodeAnalysis;

namespace Miniauth.Pages;

public static class LoginPage
{
    public static string Render(bool showCredentialError)
    {
        return string.Format(
            Layout,
            Shared.Stylesheet,
            string.Format(
                Form,
                showCredentialError ? WrongCredentialsMessage : string.Empty));
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
    private const string Form =
        """
         <form method="post" class="card">
             <header><h2>Login</h2></header>
             <label>Username <input type="text" name="username" required></label>
             <label>Password <input type="password" name="password" required></label>
             {0}
             <button type="submit">Login</button>
         </form>
         """;

    [StringSyntax("html")]
    private const string WrongCredentialsMessage =
        """
            <span class="error">Wrong username or password.</span>
        """;

}
