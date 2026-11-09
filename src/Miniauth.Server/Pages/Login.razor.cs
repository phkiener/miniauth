using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Miniauth.Server.Authentication;

namespace Miniauth.Server.Pages;

public sealed partial class Login(
    IAuthenticationProvider authenticationProvider,
    NavigationManager navigationManager,
    ILogger<Login> logger) : ComponentBase
{
    [SupplyParameterFromForm]
    public string? Username { get; set; }

    [SupplyParameterFromForm]
    public string? Password { get; set; }

    [CascadingParameter]
    public required HttpContext HttpContext { get; set; }

    private EditContext editContext = null!;
    private ValidationMessageStore validationMessageStore = null!;

    private string? errorMessage;

    protected override void OnInitialized()
    {
        editContext = new EditContext(this);
        validationMessageStore = new ValidationMessageStore(editContext);
    }

    private async Task LoginAsync()
    {
        errorMessage = null;
        validationMessageStore.Clear();

        try
        {
            var principal = await authenticationProvider.AuthenticateAsync(username: Username ?? "", password: Password ?? "");
            if (principal is null)
            {
                errorMessage = "Invalid username or password";
                validationMessageStore.Add(() => Username!, string.Empty);
                validationMessageStore.Add(() => Password!, string.Empty);

                return;
            }

            await HttpContext.SignInAsync(principal);
            navigationManager.NavigateTo("/");
        }
        catch (Exception e) when (e is not NavigationException)
        {
            logger.LogError(e, "Failed to authenticate user");

            errorMessage = "Credentials cannot be checked; try again later";
        }

    }
}
