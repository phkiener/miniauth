using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Miniauth.Server.Handlers;

public static class Challenge
{
    public static async Task Handle(
        HttpContext context,
        [FromQuery(Name = "scope")] string? scope = null,
        [FromHeader(Name = "X-Forwarded-Method")] string? forwardedMethod = null,
        [FromHeader(Name = "X-Forwarded-Proto")] string? forwardedProto = null,
        [FromHeader(Name = "X-Forwarded-Host")] string? forwardedHost = null,
        [FromHeader(Name = "X-Forwarded-Uri")] string? forwardedUri = null,
        [FromHeader(Name = "Sec-Fetch-Dest")] string? destination = null)
    {
        var properties = new AuthenticationProperties();
        var isTopLevelRequest = destination is "document";

        properties.SetParameter("allow-redirect", isTopLevelRequest);
        properties.SetParameter("method", forwardedMethod);
        properties.SetParameter("proto", forwardedProto);
        properties.SetParameter("host", forwardedHost);
        properties.SetParameter("uri", forwardedUri);

        if (context.User.Identity is null or { IsAuthenticated: false })
        {
            await context.ChallengeAsync(properties);
            return;
        }

        var hasAccess = scope is null
            ? context.User.Identity.IsAuthenticated
            : context.User.HasClaim("group", scope);

        if (!hasAccess)
        {
            await context.ForbidAsync(properties);
            return;
        }

        if (isTopLevelRequest)
        {
            context.Response.StatusCode = StatusCodes.Status200OK;
            await context.Response.WriteAsync("Congrats, you got access.");

            return;
        }

        context.Response.StatusCode = StatusCodes.Status204NoContent;
        context.Response.Headers.Append("Remote-User", context.User.FindFirstValue("name"));
    }
}
