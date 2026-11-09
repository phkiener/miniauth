using System.Diagnostics;
using System.Security.Claims;

namespace Miniauth.Server.Authentication;

public sealed class ExternalAuthenticationProvider(IConfiguration configuration, ILogger<ExternalAuthenticationProvider> logger) : IAuthenticationProvider
{
    private const string EnvVar = "MINIAUTH_CHECK";

    public async Task<ClaimsPrincipal?> AuthenticateAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        var executable = configuration.GetValue<string>(EnvVar);
        if (executable is null)
        {
            logger.LogError("Missing configuration for {EnvVar}, cannot authenticate.", EnvVar);

            return null;
        }

        var processInfo = new ProcessStartInfo(executable, [username])
        {
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            RedirectStandardInput = true
        };

        var process = Process.Start(processInfo);
        if (process is null)
        {
            logger.LogError("Could not start process '{ProcessName}'", executable);
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
