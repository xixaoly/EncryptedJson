using XiLabs.EncryptedJson.Loaders;
using Microsoft.Extensions.Configuration;
using XiLabs.EncryptedJson.Decryptors;
using System.Text;

namespace XiLabs.EncryptedJson;

public static class EncryptedEncryptedJsonConfigurationExtensions
{
    public static IConfigurationBuilder AddEncryptedJsonFile(this IConfigurationBuilder builder, string path, IDecryptor decryptor, bool optional = false)
        => builder.AddEncryptedJsonFile(new FileLoader(path), decryptor, optional);

    public static IConfigurationBuilder AddEncryptedJsonFile(this IConfigurationBuilder builder, ILoader loader, IDecryptor decryptor, bool optional = false)
    {
        try
        {
            string? content = decryptor.Decrypt(loader.Load());
            if(!string.IsNullOrEmpty(content))
            {
                builder.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(content)));
            }
        }
        catch 
        {
            if(!optional)
            {
                throw;
            }
        }

        return builder;
    }
}
