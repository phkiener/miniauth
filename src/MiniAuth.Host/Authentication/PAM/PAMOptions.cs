using System.ComponentModel.DataAnnotations;

namespace MiniAuth.Host.Authentication.PAM;

public sealed class PAMOptions
{
    [Required]
    [ConfigurationKeyName("PAM_SOCKET")]
    public required string Socket { get; set; }
}
