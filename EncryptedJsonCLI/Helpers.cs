namespace EncryptedJson.CLI;

public static class Helpers
{
    public static string ChangeFileExtension(string path, string extension)
        => Path.Combine(Path.GetDirectoryName(path) ?? "", $"{Path.GetFileNameWithoutExtension(path)}.{extension}");
}
