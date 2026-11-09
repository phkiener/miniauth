using Miniauth.Check.Crypt;

namespace TUnit.Generated;

/// <summary>
/// A fake <see cref="ICrypt"/> that always returns a fixed result.
/// </summary>
/// <param name="result">The result to return.</param>
internal sealed class DummyCrypt(string result) : ICrypt
{
    /// <inheritdoc />
    public string Crypt(string key, string salt)
    {
        return result;
    }
}
