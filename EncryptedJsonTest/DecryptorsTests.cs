using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using XiLabs.EncryptedJson.Decryptors;
using XiLabs.Helpers;

namespace EncryptedJson.Test;

[TestClass]
public class DecryptorsTests
{
    protected byte[] key = new byte[0];
    protected byte[] iv = new byte[0];
    protected X509Certificate2 certificate = default!;

    [TestInitialize]
    public void Initialize()
    {
        this.key = Convert.FromBase64String("8uoAU10teRaQJ9umCjjVjkC5PA8yBFItW2TDsWbhQME=");
        this.iv = Convert.FromBase64String("QaUqmm51M18AG6wgjNzt1w==");
        using (RSA rsa = RSA.Create())
        {
            var req = new CertificateRequest("CN=TestCertificate", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            this.certificate = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddDays(1));
        }
    }

    [TestMethod]
    public void Decrypt_ValidBytes_ReturnsDecryptedString()
    {
        // Arrange
        string plainText = "This is a secret message.";
        byte[] encryptedBytes = plainText.AesEncrypt(key, iv);
        var decryptor = new AesDecryptor(key, iv);

        // Act
        string decryptedString = decryptor.Decrypt(encryptedBytes);

        // Assert
        Assert.AreEqual(plainText, decryptedString);
    }


    [TestMethod]
    [ExpectedException(typeof(CryptographicException))]
    public void Decrypt_InvalidBase64_ThrowsException()
    {
        // Arrange
        string invalidBase64 = "This is not valid base64";
        var decryptor = new AesDecryptor(key, iv);

        // Act & Assert
        decryptor.Decrypt(invalidBase64);

    }

    [TestMethod]
    [ExpectedException(typeof(CryptographicException))]
    public void Decrypt_InvalidKeyOrIV_ThrowsException()
    {
        // Arrange
        string plainText = "This is a secret message.";
        string encryptedBase64 = Convert.ToBase64String(plainText.AesEncrypt(key, iv));

        byte[] wrongKey = new byte[key.Length];
        var decryptor = new AesDecryptor(wrongKey, iv);

        // Act & Assert (expect exception)
        decryptor.Decrypt(encryptedBase64);
    }

    [TestMethod]
    public void Decrypt_ValidEncryptedData_ReturnsDecryptedString()
    {
        // Arrange
        string plainText = "This is a secret message for RSA.";
        string encryptedBase64 = Convert.ToBase64String(plainText.RsaEncrypt(certificate));
        var decryptor = new RsaDecryptor(certificate);

        // Act
        string decryptedString = decryptor.Decrypt(encryptedBase64);

        // Assert
        Assert.AreEqual(plainText, decryptedString);
    }

    [TestMethod]
    [ExpectedException(typeof(FormatException))]
    public void Decrypt_InvalidEncryptedData_ThrowsCryptographicException()
    {
        // Arrange
        string invalidBase64 = "Invalid base64 string here";
        var decryptor = new RsaDecryptor(certificate);

        // Act & Assert
        decryptor.Decrypt(invalidBase64);
    }


    [TestMethod]
    [ExpectedException(typeof(Exception), "Certificate does't contain RSA private key")]
    public void Decrypt_CertificateWithoutPrivateKey_ThrowsException()
    {
        // Arrange
        X509Certificate2 publicKeyOnlyCert = new X509Certificate2(certificate.RawData);

        string plainText = "This will fail.";
        byte[] encryptedData = plainText.RsaEncrypt(certificate);
        var decryptor = new RsaDecryptor(publicKeyOnlyCert);

        // Act & Assert
        decryptor.Decrypt(encryptedData);
    }
}
