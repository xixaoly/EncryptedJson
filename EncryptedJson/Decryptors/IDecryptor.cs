namespace XiLabs.EncryptedJson.Decryptors;

public interface IDecryptor
{
    public string Decrypt(string base64);
    public string Decrypt(byte[] bytes);
}
