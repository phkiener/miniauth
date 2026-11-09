using Microsoft.AspNetCore.Authentication;

namespace Miniauth.Server.Authentication.Identity;

public sealed class SessionCookieOptions
{
    public string CookieName { get; set; } = ".miniauth";
    public TimeSpan CookieMaxAge { get; set; } = TimeSpan.FromDays(7);
    public string? CookieDomain { get; set; }

    public IDataSerializer<AuthenticationTicket> TicketSerializer { get; set; } = new AuthenticationTicketSerializer();
}
