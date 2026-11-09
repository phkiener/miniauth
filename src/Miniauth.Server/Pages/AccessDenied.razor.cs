using Microsoft.AspNetCore.Components;

namespace Miniauth.Server.Pages;

public sealed partial class AccessDenied : ComponentBase
{
    [SupplyParameterFromQuery(Name = "method")]
    public string? Method { get; set; }

    [SupplyParameterFromQuery(Name = "proto")]
    public string? Protocol { get; set; }

    [SupplyParameterFromQuery(Name = "host")]
    public string? Host { get; set; }

    [SupplyParameterFromQuery(Name = "uri")]
    public string? Uri { get; set; }

    private string fullRequest = "";

    protected override void OnParametersSet()
    {
        fullRequest = $"{Method} {Protocol}://{Host}/{Uri}";
    }
}
