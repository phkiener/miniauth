using System.Diagnostics.CodeAnalysis;

namespace Miniauth.Pages;

public static class Shared
{
    [StringSyntax("html")]
    public const string Stylesheet =
        """
        <style>
            body {
                color-scheme: light dark;

                color: light-dark(#000000, #FFFFFF);
                background: light-dark(#FFFFFF, #121212);

                margin: 0;

                height: 100dvh;
                width: 100dvw;

                display: flex;
                align-items: center;
                justify-content: center;

                font-family: sans-serif;
            }

            .card {
                background: light-dark(#F5F5F5, #1E1E1E);

                border: 1px solid light-dark(#000000, #FFFFFF);
                border-radius: 1rem;

                width: 60ch;
                height: max-content;

                padding: 1.25rem 1.5rem;

                display: flex;
                flex-direction: column;
                justify-content: center;
                align-items: stretch;
                gap: 1.75rem;

                h2 {
                    margin: 0;
                    border-bottom: 1px solid light-dark(#000000, #FFFFFF);
                }

                label {
                    display: flex;
                    flex-direction: column;
                    gap: 0.5rem;
                }

                input {
                    border: 1px solid light-dark(#000000, #FFFFFF);
                    border-radius: 0.25rem;

                    height: 1.5rem;

                    color: light-dark(#000000, #FFFFFF);
                    background: light-dark(#FFFFFF, #121212);
                }

                button {
                    border: 1px solid light-dark(#000000, #FFFFFF);
                    border-radius: 0.25rem;

                    height: 2rem;
                    align-self: stretch;

                    color: light-dark(#000000, #FFFFFF);
                    background: light-dark(#FFFFFF, #121212);

                    font-weight: bold;
                    text-transform: uppercase;

                    &:hover {
                        background: light-dark(#F8F8F8, #1A1A1A);
                    }
                }
            }

            .error { color: red; }
        </style>
        """;
}
