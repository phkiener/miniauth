using System.Diagnostics;
using Miniauth.Session;

namespace Miniauth.Authentication;

public sealed class ExternalAuthenticationProvider(IConfiguration configuration, ILogger<ExternalAuthenticationProvider> logger) : IAuthenticationProvider
{
    private const string EnvVar = "MINIAUTH_CHECK";

    public async Task<UserSession?> AuthenticateAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        var executable = configuration.GetValue<string>(EnvVar);
        if (executable is null)
        {
            logger.LogError("Missing configuration for {EnvVar}, cannot authenticate.", EnvVar);

            return null;
        }

        var processInfo = new ProcessStartInfo(executable, [username, password])
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

        var stdOut = process.StandardOutput.ReadToEndAsync(cancellationToken);
        var stdErr = process.StandardError.ReadToEndAsync(cancellationToken);
        var exited = process.WaitForExitAsync(cancellationToken);

        await Task.WhenAll(stdOut, stdErr, exited);
        if (process.ExitCode != 0)
        {
            return null;
        }

        var groups = (await stdOut).Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        return new UserSession(username, groups);
    }
}
