using System.Security.Cryptography;
using System.Text;

namespace FlowFeedback.Infrastructure.Security;

public interface ICryptoService
{
    string Encrypt(string plainText);
    string Decrypt(string cipherText);
}

public class CryptoService : ICryptoService
{
    private readonly byte[] _key;
    private const int IvSize = 16;

    public CryptoService(string masterKey)
    {
        _key = Encoding.UTF8.GetBytes(masterKey.PadRight(32).Substring(0, 32));
    }

    public string Encrypt(string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.GenerateIV();
        var iv = aes.IV;

        using var encryptor = aes.CreateEncryptor(aes.Key, iv);
        using var ms = new MemoryStream();

        ms.Write(iv, 0, iv.Length);

        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var sw = new StreamWriter(cs))
        {
            sw.Write(plainText);
        }

        return Convert.ToBase64String(ms.ToArray());
    }

    public string Decrypt(string cipherText)
    {
        var fullCipher = Convert.FromBase64String(cipherText);
        using var aes = Aes.Create();
        aes.Key = _key;

        var iv = new byte[IvSize];
        var cipher = new byte[fullCipher.Length - IvSize];

        Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
        Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, cipher.Length);

        using var decryptor = aes.CreateDecryptor(aes.Key, iv);
        using var ms = new MemoryStream(cipher);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);

        return sr.ReadToEnd();
    }
}