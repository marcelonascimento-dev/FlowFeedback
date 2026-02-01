using FlowFeedback.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

// Configurar o builder de configuração
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true)
    .AddJsonFile("appsettings.Development.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

Console.WriteLine("========================================");
Console.WriteLine("   FlowFeedback Cryptography Utility   ");
Console.WriteLine("========================================");

if (args.Length == 0)
{
    Console.WriteLine("\nUsage: FlowFeedback.Utils <text_to_encrypt> [master_key]");
    Console.WriteLine("\nExample:");
    Console.WriteLine("  dotnet run -- \"my_password\"");
    Console.WriteLine("  dotnet run -- \"my_password\" \"my_custom_master_key\"");
    return;
}

string textToEncrypt = args[0];

// 1. Prioridade: Argumento CLI
// 2. Segunda: Variável de Ambiente / Config (Crypto:MasterKey)
// 3. Fallback: Valor padrão do desenvolvimento
string masterKey = args.Length > 1
    ? args[1]
    : (config["Crypto:MasterKey"] ?? "$e$fQ+9!h@mGTU9=cye(dELZYBI9Z_Au");

try
{
    var cryptoService = new CryptoService(masterKey);
    string encrypted = cryptoService.Encrypt(textToEncrypt);

    Console.WriteLine($"\nOriginal text: {textToEncrypt}");
    Console.WriteLine($"Master Key Used: {(args.Length > 1 ? "(CLI Argument)" : config["Crypto:MasterKey"] != null ? "(Config/EnvVar)" : "(Default Dev Key)")}");

    Console.WriteLine("\nEncrypted String (use this in the database):");
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(encrypted);
    Console.ResetColor();
    Console.WriteLine("\n========================================");
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"\nError during encryption: {ex.Message}");
    Console.ResetColor();
}
