using Miniauth.Check.Crypt;
using Miniauth.Check.Files;

namespace Miniauth.Check;

public abstract record CheckResult
{
    protected internal CheckResult() { /* Poor man's union until .NET11 */ }

    public sealed record Success(string[] Groups) : CheckResult;
    public sealed record Failure : CheckResult;
}

public sealed class PasswordChecker(ICrypt crypt, string shadowFile, string groupFile)
{
    public CheckResult Check(string username, string password)
    {
        var passwordHash = ShadowFile.FindPasswordHash(username, path: shadowFile);

        var generatedHash = crypt.Crypt(passwordHash ?? "", password);
        if (generatedHash != passwordHash)
        {
            return new CheckResult.Failure();
        }

        var groups = GroupFile.FindGroups(username, path: groupFile);
        return new CheckResult.Success([.. groups]);
    }
}
