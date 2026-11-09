using System.ComponentModel.DataAnnotations;

namespace Miniauth.Server;

public sealed class MiniauthOptions
{
    [Required]
    [ConfigurationKeyName("MINIAUTH_DOMAIN")]
    public required string Domain { get; set; }

    [Range(minimum: 1, maximum: 14)]
    [ConfigurationKeyName("MINIAUTH_SESSIONLIFETIMEDAYS")]
    public int SessionLifetimeDays { get; set; } = 7;

    [Required]
    [ConfigurationKeyName("MINIAUTH_EXECUTABLE")]
    public required string Executable { get; set; }
}
