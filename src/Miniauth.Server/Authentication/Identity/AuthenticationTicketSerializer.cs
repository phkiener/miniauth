using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;

namespace Miniauth.Server.Authentication.Identity;

public sealed class AuthenticationTicketSerializer : IDataSerializer<AuthenticationTicket>
{
    public byte[] Serialize(AuthenticationTicket model)
    {
        var username = model.Principal.FindFirstValue("name");
        var groups = model.Principal.FindAll("group").Select(static c => c.Value);
        var type = model.Principal.Identity?.Name;

        var serialized = $"{model.AuthenticationScheme}:{type}:{username}:{string.Join(',', groups)}";
        return Encoding.UTF8.GetBytes(serialized);
    }

    public AuthenticationTicket? Deserialize(byte[] data)
    {
        var serialized = Encoding.UTF8.GetString(data);
        if (serialized.Split(':') is not [var scheme, var type, var username, var groups])
        {
            return null;
        }

        var identity = new ClaimsIdentity(type);
        identity.AddClaim(new Claim("name", username));
        identity.AddClaims(groups.Split(',').Select(static g => new Claim("group", g)));

        var principal = new ClaimsPrincipal(identity);
        return new AuthenticationTicket(principal, scheme);
    }
}
