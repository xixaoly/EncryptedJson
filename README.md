# .NET Encrypted JSON configuration extension
This library extends `ConfigurationBuilder` about encrypted json files. RSA and AES supported.

## AES example
```
var builder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .AddEncryptedJsonFile(
        "appsettings.aes.bin",
        new AesDecryptor(Convert.FromBase64String("8uoAU10teRaQJ9umCjjVjkC5PA8yBFItW2TDsWbhQME="), Convert.FromBase64String("QaUqmm51M18AG6wgjNzt1w==")),
        optional: true);
var configuration = builder.Build();

Config config = configuration.Get<Config>() ?? new Config();
```

## RSA example
```
var builder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .AddEncryptedJsonFile("appsettings.rsa.bin", new RsaDecryptor(certificate), optional: true);
var configuration = builder.Build();

Config config = configuration.Get<Config>() ?? new Config();
```

## Tool for encryption
```
# install
dotnet pack EncryptedJsonCLI
dotnet tool install --global --add-source ./EncryptedJsonCLI/nupkg EncryptedJson.CLI

# run
encryptedjson -h

# uninstall
dotnet tool uninstall -g EncryptedJson.CLI
```