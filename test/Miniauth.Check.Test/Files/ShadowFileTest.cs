using Miniauth.Check.Files;

namespace TUnit.Generated.Files;

public sealed class ShadowFileTest
{
    [Test]
    public async Task ReturnsPasswordHash_ForUser()
    {
        using var file = TempFile.Create("etc-shadow", "foobar:$0$dummy$hash$:::::::");

        var hash = ShadowFile.FindPasswordHash("foobar", path: file);
        await Assert.That(hash).IsEqualTo("$0$dummy$hash$");
    }

    [Test]
    public async Task ReturnsNull_WhenUserNotFound()
    {
        using var file = TempFile.Create("etc-shadow", "foobar:$0$dummy$hash$:::::::");

        var hash = ShadowFile.FindPasswordHash("some.guy", path: file);
        await Assert.That(hash).IsNull();
    }

    [Test]
    public async Task Ignores_Malformed_Lines()
    {
        using var file = TempFile.Create("etc-shadow",
            """
            # this is not a comment, but just pretend it is
            foobar:$0$not-enough$fields$
            foobar:$0$too-many$fields$::::::::
            foobar:$0$dummy$hash$:::::::
            """);

        var hash = ShadowFile.FindPasswordHash("foobar", path: file);
        await Assert.That(hash).IsEqualTo("$0$dummy$hash$");
    }
}
