using System.Security.Cryptography;
using System.Text;

namespace XiLabs.Helpers;

public static class AesHelper
{
    public static byte[] AesEncrypt(this string text, byte[] key, byte[] iv)
        => Encoding.UTF8.GetBytes(text).AesEncrypt(key, iv);

    public static byte[] AesEncrypt(this byte[] bytes, byte[] key, byte[] iv)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(bytes, 0, bytes.Length);
                    cs.FlushFinalBlock();
                    return ms.ToArray();
                }
            }
        }
    }

    public static string AesDecrypt(this string base64, byte[] key, byte[] iv)
        => Convert.FromBase64String(base64).AesDecrypt(key, iv);

    public static string AesDecrypt(this byte[] bytes, byte[] key, byte[] iv)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream ms = new MemoryStream(bytes))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cs))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }
    }
}
