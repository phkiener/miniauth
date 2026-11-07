using System.Runtime.InteropServices;

namespace Miniauth.Check;

public static partial class PasswordCheck
{
    public static bool CheckPassword(string? hash, string password)
    {
        var result = Crypt(password, hash ?? "");
        return result == hash;
    }

    [LibraryImport("libcrypt", EntryPoint = "crypt", StringMarshalling = StringMarshalling.Utf8)]
    private static partial string Crypt(string phrase, string setting);
}
