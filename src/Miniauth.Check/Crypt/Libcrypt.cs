using System.Runtime.InteropServices;

namespace Miniauth.Check.Crypt;

/// <summary>
/// An <see cref="ICrypt"/> implementatio delegating to <em>libcrypt</em>, usually <em>libxcrypt</em> to be precise.
/// </summary>
public sealed partial class Libcrypt : ICrypt
{
    /// <summary>
    /// A shared instance of <see cref="Libcrypt"/>.
    /// </summary>
    public static readonly ICrypt Instance = new Libcrypt();

    /// <inheritdoc />
    public string Crypt(string password, string hash)
    {
        return InvokeCrypt(password, hash);
    }

    [LibraryImport("libcrypt", EntryPoint = "crypt", StringMarshalling = StringMarshalling.Utf8)]
    private static partial string InvokeCrypt(string phrase, string setting);

}
