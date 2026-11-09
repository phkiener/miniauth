using System.Diagnostics;
using System.Security.Claims;
using Microsoft.Extensions.Options;

namespace Miniauth.Server.Authentication.Identity;

public sealed class ExternalIdentityProvider(IOptions<MiniauthOptions> options, ILogger<ExternalIdentityProvider> logger) : IIdentityProvider
{
    public async Task<ClaimsPrincipal?> LoginAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        var processInfo = new ProcessStartInfo(options.Value.Executable, [username])
        {
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            RedirectStandardInput = true
        };

        var process = Process.Start(processInfo);
        if (process is null)
        {
            logger.LogError("Could not start process '{ProcessName}'", options.Value.Executable);
            return null;
        }

        await process.StandardInput.WriteLineAsync(password);

        var stdOut = process.StandardOutput.ReadToEndAsync(cancellationToken);
        var stdErr = process.StandardError.ReadToEndAsync(cancellationToken);
        var exited = process.WaitForExitAsync(cancellationToken);

        await Task.WhenAll(stdOut, stdErr, exited);
        if (process.ExitCode != 0)
        {
            return null;
        }

        var groups = (await stdOut).Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var identity = new ClaimsIdentity("miniauth");
        identity.AddClaim(new Claim("name", username));
        identity.AddClaims(groups.Select(static g => new Claim("group", g)));

        return new ClaimsPrincipal(identity);
    }
}
