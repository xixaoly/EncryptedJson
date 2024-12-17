
namespace XiLabs.EncryptedJson.Loaders;

public class Base64Loader : ILoader
{
    protected string content;

    public Base64Loader(string content)
    {
        this.content = content;
    }

    public byte[] Load()
        => Convert.FromBase64String(this.content);
}
