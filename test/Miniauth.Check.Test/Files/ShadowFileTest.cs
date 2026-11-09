using Miniauth.Check.Files;

namespace TUnit.Generated.Files;

public sealed class ShadowFileTest
{
    [Test]
    public async Task ReturnsPasswordHash_ForUser()
    {
        var filePath = MakeTempFile("foobar:$0$dummy$hash$:::::::");

        var hash = ShadowFile.FindPasswordHash("foobar", path: filePath);
        await Assert.That(hash).IsEqualTo("$0$dummy$hash$");
    }

    [Test]
    public async Task ReturnsNull_WhenUserNotFound()
    {
        var filePath = MakeTempFile("foobar:$0$dummy$hash$:::::::");

        var hash = ShadowFile.FindPasswordHash("some.guy", path: filePath);
        await Assert.That(hash).IsNull();
    }

    [Test]
    public async Task Ignores_Malformed_Lines()
    {
        var filePath = MakeTempFile(
            """
            # this is not a comment, but just pretend it is
            foobar:$0$not-enough$fields$
            foobar:$0$too-many$fields$::::::::
            foobar:$0$dummy$hash$:::::::
            """);

        var hash = ShadowFile.FindPasswordHash("foobar", path: filePath);
        await Assert.That(hash).IsEqualTo("$0$dummy$hash$");
    }

    private string MakeTempFile(string content)
    {
        var directory = Directory.CreateTempSubdirectory("miniauth-test");
        var filename = $"etc-shadow-{DateTime.UtcNow:yyyy-MM-dd--hh-mm-ss}-{Guid.NewGuid():N}";

        var filePath = Path.Combine(directory.FullName, filename);
        File.WriteAllText(filePath, content);

        return filePath;
    }
}
