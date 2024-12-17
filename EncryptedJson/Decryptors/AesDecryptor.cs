
using XiLabs.Helpers;

namespace XiLabs.EncryptedJson.Decryptors;

public class AesDecryptor : IDecryptor
{
    protected byte[] key;
    protected byte[] iv;

    public AesDecryptor(byte[] key, byte[] iv)
    {
        this.key = key;
        this.iv = iv;
    }

    public string Decrypt(string base64)
        => base64.AesDecrypt(this.key, this.iv);

    public string Decrypt(byte[] bytes)
        => bytes.AesDecrypt(this.key, this.iv);
}
