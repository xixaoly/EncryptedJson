
using System.Security.Cryptography.X509Certificates;
using XiLabs.Helpers;

namespace XiLabs.EncryptedJson.Decryptors;

public class RsaDecryptor : IDecryptor
{
    protected X509Certificate2 cert;

    public RsaDecryptor(X509Certificate2 cert)
    {
        this.cert = cert;
    }

    public string Decrypt(string base64)
        => base64.RsaDecrypt(this.cert);

    public string Decrypt(byte[] bytes)
        => bytes.RsaDecrypt(this.cert);
}
