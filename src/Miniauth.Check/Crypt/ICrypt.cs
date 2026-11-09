namespace Miniauth.Check.Crypt;

/// <summary>
/// A wrapper to invoke crypt(3).
/// </summary>
public interface ICrypt
{
    /// <summary>
    /// Invoke crypt(3) with the given <paramref name="key"/> and <paramref name="salt"/>.
    /// </summary>
    /// <param name="key">The key (or password) to hash.</param>
    /// <param name="salt">The salt (or existing hash in case of libxcrypt) to use.</param>
    /// <returns>The hashed <paramref name="key"/>.</returns>
    public string Crypt(string key, string salt);
}
