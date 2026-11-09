namespace TUnit.Generated;

/// <summary>
/// A temporary file that is deleted on <see cref="Dispose"/>.
/// </summary>
public sealed class TempFile : IDisposable
{
    private readonly string filePath;

    private TempFile(string filePath)
    {
        this.filePath = filePath;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        File.Delete(filePath);
    }

    /// <summary>
    /// Cast this file to a string containing the full path to the file.
    /// </summary>
    /// <param name="file">The file to cast.</param>
    /// <returns>The file's path.</returns>
    public static implicit operator string(TempFile file) => file.filePath;

    /// <summary>
    /// Create a new <see cref="TempFile"/>.
    /// </summary>
    /// <param name="label">A label to identify the file.</param>
    /// <param name="content">The content of the file.</param>
    /// <returns>The created temporary file.</returns>
    public static TempFile Create(string label, string content)
    {
        var directory = Directory.CreateTempSubdirectory("miniauth-test");
        var filename = $"{label}-{DateTime.UtcNow:yyyy-MM-dd--hh-mm-ss}-{Guid.NewGuid():N}";

        var filePath = Path.Combine(directory.FullName, filename);
        File.WriteAllText(filePath, content);

        return new TempFile(filePath);
    }
}
