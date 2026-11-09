using System.Runtime.InteropServices;

namespace Miniauth.Check;

public static partial class Crypt
{
    public static bool CheckPassword(string? hash, string password)
    {
        var result = InvokeCrypt(password, hash ?? "");
        return result == hash;
    }

    [LibraryImport("libcrypt", EntryPoint = "crypt", StringMarshalling = StringMarshalling.Utf8)]
    private static partial string InvokeCrypt(string phrase, string setting);
}
