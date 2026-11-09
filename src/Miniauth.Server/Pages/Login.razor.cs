using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;
using Miniauth.Server.Authentication;

namespace Miniauth.Server.Pages;

public sealed partial class Login(
    IIdentityProvider authenticationProvider,
    NavigationManager navigationManager,
    IOptions<MiniauthOptions> options,
    ILogger<Login> logger) : ComponentBase
{
    [SupplyParameterFromQuery(Name = "method")]
    public string? Method { get; set; }

    [SupplyParameterFromQuery(Name = "proto")]
    public string? Protocol { get; set; }

    [SupplyParameterFromQuery(Name = "host")]
    public string? Host { get; set; }

    [SupplyParameterFromQuery(Name = "uri")]
    public string? Uri { get; set; }

    [SupplyParameterFromForm]
    public string? Username { get; set; }

    [SupplyParameterFromForm]
    public string? Password { get; set; }

    [CascadingParameter]
    public required HttpContext HttpContext { get; set; }

    [CascadingParameter]
    public required Task<AuthenticationState> AuthenticationStateTask { get; set; }

    private EditContext editContext = null!;
    private ValidationMessageStore validationMessageStore = null!;

    private string? errorMessage;

    protected override void OnInitialized()
    {
        editContext = new EditContext(this);
        validationMessageStore = new ValidationMessageStore(editContext);
    }

    protected override async Task OnInitializedAsync()
    {
        var authenticationState = await AuthenticationStateTask;
        if (authenticationState.User.Identity?.IsAuthenticated is true)
        {
            logger.LogInformation("User {Username} has hit /login despite being logged in", Username);
            navigationManager.NavigateTo(GetRedirectTarget());
        }
    }

    private async Task LoginAsync()
    {
        errorMessage = null;
        validationMessageStore.Clear();

        try
        {
            var principal = await authenticationProvider.LoginAsync(username: Username ?? "", password: Password ?? "");
            if (principal is null)
            {
                errorMessage = "Invalid username or password";
                validationMessageStore.Add(() => Username!, string.Empty);
                validationMessageStore.Add(() => Password!, string.Empty);

                return;
            }

            await HttpContext.SignInAsync(principal);
            logger.LogInformation("Logged in user {Username} from {IP}", Username, HttpContext.Connection.RemoteIpAddress);

            navigationManager.NavigateTo(GetRedirectTarget());
        }
        catch (Exception e) when (e is not NavigationException)
        {
            logger.LogError(e, "Failed to authenticate user");

            errorMessage = "Credentials cannot be checked; try again later";
        }
    }

    private string GetRedirectTarget()
    {
        // We can't handle non-GET requests properly. If not specified, we just assume GET.
        if (Method is not null && !Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
        {
            return "/";
        }

        // When the host can't see the cookie anyway, we would just cause an infinite loop. So we don't.
        // If the host isn't specified at all, something is off.
        var allowedDomain = options.Value.Domain;
        if (Host is null || !Host.EndsWith(allowedDomain))
        {
            return "/";
        }

        return $"{Protocol}://{Host}{Uri}";
    }
}
