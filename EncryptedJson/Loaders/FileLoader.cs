using System.Text;

namespace XiLabs.EncryptedJson.Loaders;

public class FileLoader : ILoader
{
    protected string path;

    public FileLoader(string path)
    {
        this.path = path;
    }

    public byte[] Load()
    {
        if(File.Exists(path))
        {
            return File.ReadAllBytes(this.path);
        }
        else
        {
            throw new Exception($"Can't find file {path}");
        }
    }
}
