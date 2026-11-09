using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Miniauth.Server.Pages;

public sealed partial class Index(NavigationManager navigationManager, ILogger<Index> logger) : ComponentBase
{
    [CascadingParameter]
    public required HttpContext HttpContext { get; set; }

    [CascadingParameter]
    public required Task<AuthenticationState> AuthenticationStateTask { get; set; }

    private string? username;
    private string[] groups = [];

    protected override async Task OnInitializedAsync()
    {
        var authenticationState = await AuthenticationStateTask;

        username = authenticationState.User.FindFirstValue("name");
        groups = authenticationState.User.FindAll("group").Select(static c => c.Value).ToArray();
    }

    private async Task LogoutAsync()
    {
        await HttpContext.SignOutAsync();
        logger.LogInformation("Explicitly logged out user {Username} from {IP}", username, HttpContext.Connection.RemoteIpAddress);

        navigationManager.NavigateTo("/");
    }
}
