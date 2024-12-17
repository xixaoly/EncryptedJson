using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace XiLabs.Helpers;

public static class RsaHelper
{
    public static byte[] RsaEncrypt(this string text, X509Certificate2 cert)
        => Encoding.UTF8.GetBytes(text).RsaEncrypt(cert);

    public static byte[] RsaEncrypt(this byte[] bytes, X509Certificate2 cert)
    {
        RSA? rsa = cert.GetRSAPublicKey();
        if(rsa != null)
        {
            return rsa.Encrypt(bytes, RSAEncryptionPadding.Pkcs1);
        }

        throw new Exception("Certificate does't contain RSA public key");
    }

    public static string RsaDecrypt(this string base64, X509Certificate2 cert)
        => Convert.FromBase64String(base64).RsaDecrypt(cert);

    public static string RsaDecrypt(this byte[] bytes, X509Certificate2 cert)
    {
        var rsa = cert.GetRSAPrivateKey();
        if(rsa != null)
        {
            byte[] decryptedData = rsa.Decrypt(bytes, RSAEncryptionPadding.Pkcs1);
            return Encoding.UTF8.GetString(decryptedData);
        }

        throw new Exception("Certificate does't contain RSA private key");
    }
}
