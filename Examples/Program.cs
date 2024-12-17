using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using EncryptedJson;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using XiLabs.EncryptedJson;
using XiLabs.EncryptedJson.Decryptors;
using XiLabs.EncryptedJson.Loaders;
using XiLabs.Helpers;

#region Mocks
string secretJson = """{"encryptedKey":"secret"}""";

File.WriteAllBytes("appsettings.aes.bin", secretJson.AesEncrypt(Convert.FromBase64String("8uoAU10teRaQJ9umCjjVjkC5PA8yBFItW2TDsWbhQME="), Convert.FromBase64String("QaUqmm51M18AG6wgjNzt1w==")));

X509Certificate2 certificate = default!;
using (RSA rsa = RSA.Create())
{
    CertificateRequest req = new CertificateRequest("CN=TestCertificate", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    certificate = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddDays(1));
}
File.WriteAllBytes("appsettings.rsa.bin", secretJson.RsaEncrypt(certificate));
#endregion

#region AES encrypted file
var builder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
var configuration = builder.Build();

Config config = configuration.Get<Config>() ?? new Config();
Console.WriteLine($"Public config: {JsonConvert.SerializeObject(config)}");
#endregion

#region AES encrypted file
builder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .AddEncryptedJsonFile(
        "appsettings.aes.bin",
        new AesDecryptor(Convert.FromBase64String("8uoAU10teRaQJ9umCjjVjkC5PA8yBFItW2TDsWbhQME="), Convert.FromBase64String("QaUqmm51M18AG6wgjNzt1w==")),
        optional: true);
configuration = builder.Build();

config = configuration.Get<Config>() ?? new Config();
Console.WriteLine($"AES file secret config: {JsonConvert.SerializeObject(config)}");
#endregion

#region AES encrypted base64
builder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .AddEncryptedJsonFile(
        new Base64Loader("PJiU93YDeQ/I2f0PMGxJmtEaXkMoELX+01lLj/olaPw="),
        new AesDecryptor(Convert.FromBase64String("8uoAU10teRaQJ9umCjjVjkC5PA8yBFItW2TDsWbhQME="), Convert.FromBase64String("QaUqmm51M18AG6wgjNzt1w==")),
        optional: true);
configuration = builder.Build();

config = configuration.Get<Config>() ?? new Config();
Console.WriteLine($"AES base64 secret config: {JsonConvert.SerializeObject(config)}");
#endregion

#region RSA encrypted file
builder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .AddEncryptedJsonFile("appsettings.rsa.bin", new RsaDecryptor(certificate), optional: true);
configuration = builder.Build();

config = configuration.Get<Config>() ?? new Config();
Console.WriteLine($"RSA file secret config: {JsonConvert.SerializeObject(config)}");
#endregion
