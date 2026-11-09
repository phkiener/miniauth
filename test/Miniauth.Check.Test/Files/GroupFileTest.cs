using Miniauth.Check.Files;

namespace TUnit.Generated.Files;

public sealed class GroupFileTest
{
    [Test]
    public async Task ReturnsPasswordHash_ForUser()
    {
        using var file = TempFile.Create("etc-group",
            """
            a::1:foobar,dummy
            b::2:dummy,foobar
            c:*:3:dummy
            d:*:4:foobar
            """);

        var groups = GroupFile.FindGroups("foobar", path: file);
        await Assert.That(groups).IsEquivalentTo(["a", "b", "d"]);
    }

    [Test]
    public async Task ReturnsNull_WhenUserNotFound()
    {
        using var file = TempFile.Create("etc-group",
            """
            a::1:foobar,dummy
            b::2:dummy,foobar
            c:*:3:dummy
            d:*:4:foobar
            """);

        var groups = GroupFile.FindGroups("some.guy", path: file);
        await Assert.That(groups).IsEmpty();
    }

    [Test]
    public async Task Ignores_Malformed_Lines()
    {
        using var file = TempFile.Create("etc-group",
            """
            # not really a comment
            too-short:
            too-long::5:foobar:
            d::4:foobar
            """);

        var groups = GroupFile.FindGroups("foobar", path: file);
        await Assert.That(groups).IsEquivalentTo(["d"]);
    }
}
