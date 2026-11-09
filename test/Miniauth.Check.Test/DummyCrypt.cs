using Miniauth.Check.Crypt;

namespace TUnit.Generated;

/// <summary>
/// A fake <see cref="ICrypt"/> that always returns the salt - if the key matches the given key.
/// </summary>
/// <param name="expectedKey">The key for which to return back the existing salt.</param>
internal sealed class DummyCrypt(string expectedKey) : ICrypt
{
    /// <inheritdoc />
    public string Crypt(string key, string salt)
    {
        return key == expectedKey ? salt : "nope";
    }
}
