using Miniauth.Check;

namespace TUnit.Generated;

public sealed class PasswordCheckerTest
{
    [Test]
    public async Task ReturnsSuccess_WhenPasswordMatchesHash()
    {
        using var shadowFile = TempFile.Create("etc-shadow", "foobar:$0$dummy$hash$:::::::");
        using var groupFile = TempFile.Create("etc-group", "group::1:foobar");

        var crypt = new DummyCrypt("$0$dummy$hash$");
        var checker = new PasswordChecker(crypt: crypt, shadowFile: shadowFile, groupFile: groupFile);

        var result = checker.Check("foobar", "password");
        await Assert.That(result).IsTypeOf<CheckResult.Success>().And.Satisfies(s => s?.Groups is ["group"]);
    }

    [Test]
    public async Task ReturnsFailure_WhenPasswordDoesNotMatch()
    {
        using var shadowFile = TempFile.Create("etc-shadow", "foobar:$0$dummy$hash$:::::::");
        using var groupFile = TempFile.Create("etc-group", "group::1:foobar");

        var crypt = new DummyCrypt("$0$different$hash$");
        var checker = new PasswordChecker(crypt: crypt, shadowFile: shadowFile, groupFile: groupFile);

        var result = checker.Check("foobar", "password");
        await Assert.That(result).IsTypeOf<CheckResult.Failure>();
    }
}
