using System.ComponentModel.DataAnnotations;

namespace Miniauth.Server.Authentication;

public sealed class IdentityOptions
{
    [Required]
    [ConfigurationKeyName("MINIAUTH_DOMAIN")]
    public required string Domain { get; set; }

    [Required]
    [ConfigurationKeyName("MINIAUTH_LOGINHOST")]
    public required string LoginHost { get; set; }

    [Range(minimum: 1, maximum: 14)]
    [ConfigurationKeyName("MINIAUTH_SESSIONLIFETIMEDAYS")]
    public int SessionLifetimeDays { get; set; } = 7;
}
