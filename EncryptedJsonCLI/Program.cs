using System.Security.Cryptography.X509Certificates;
using EncryptedJson.CLI;
using McMaster.Extensions.CommandLineUtils;
using XiLabs.Helpers;

var app = new CommandLineApplication
{
    Name = "encrypted-json",
    Description = "Tool for json config files encryption",
};

app.HelpOption(inherited: true);

app.Command("aes", cmd =>
{
    cmd.Description = "Encrypt configuration with AES";
    var inputSource = cmd.Argument("input", "Input file").IsRequired();
    var keySource = cmd.Argument("key", "Key in base64").IsRequired();
    var ivSource = cmd.Argument("iv", "IV in base64").IsRequired();

    var outputSource = cmd.Option("-o|--output", "Output file", CommandOptionType.SingleOrNoValue);
    var testSource = cmd.Option("-s|--show", "Show decrypted file", CommandOptionType.NoValue);

    cmd.OnExecute(() =>
    {
        try
        {
            string output = !string.IsNullOrEmpty(outputSource.Value())
                ? outputSource.Value()!
                : Helpers.ChangeFileExtension(inputSource.Value!, "bin");
            string source = File.ReadAllText(inputSource.Value!);
            byte[] key = Convert.FromBase64String(keySource.Value!);
            byte[] iv = Convert.FromBase64String(ivSource.Value!);

            Console.WriteLine($"Creating encrypted configuration '{output}'");
            byte[] binary = source.AesEncrypt(key, iv);
            File.WriteAllBytes(output, binary);
            Console.WriteLine("Done");

            if(testSource.HasValue())
            {
                Console.WriteLine("Encrypted:");
                string plain = Convert.ToBase64String(binary).AesDecrypt(key, iv);
                Console.WriteLine(plain);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    });

    cmd.OnValidationError((e) => {
        Console.WriteLine(e.ErrorMessage);
        cmd.ShowHelp();
        return 1;
    });
});

app.Command("rsa", cmd =>
{
    cmd.Description = "Encrypt configuration with RSA";
    var inputSource = cmd.Argument("input", "Input file").IsRequired();
    var pfxSource = cmd.Argument("pfx", "PFX file").IsRequired();

    var passwdSource = cmd.Option("-p|--password", "PFX password", CommandOptionType.SingleOrNoValue);
    var outputSource = cmd.Option("-o|--output", "Output file", CommandOptionType.SingleOrNoValue);
    var testSource = cmd.Option("-s|--show", "Show decrypted file", CommandOptionType.NoValue);

    cmd.OnExecute(() =>
    {
        try
        {
            string output = !string.IsNullOrEmpty(outputSource.Value())
                ? outputSource.Value()!
                : Helpers.ChangeFileExtension(inputSource.Value!, "bin");
            string source = File.ReadAllText(inputSource.Value!);

            Console.WriteLine($"Creating encrypted configuration '{output}'");

            X509Certificate2 cert = new X509Certificate2(pfxSource.Value!, passwdSource.Value());
            var binary = source.RsaEncrypt(cert);
            File.WriteAllBytes(output, binary);
            Console.WriteLine("Done");

            if(testSource.HasValue())
            {
                Console.WriteLine("Encrypted:");
                var plain = Convert.ToBase64String(binary).RsaDecrypt(cert);
                Console.WriteLine(plain);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    });

    cmd.OnValidationError((e) => {
        Console.WriteLine(e.ErrorMessage);
        cmd.ShowHelp();
        return 1;
    });
});

app.OnExecute(() =>
{
    Console.WriteLine("Specify a subcommand");
    app.ShowHelp();
    return 1;
});

app.Execute(args);