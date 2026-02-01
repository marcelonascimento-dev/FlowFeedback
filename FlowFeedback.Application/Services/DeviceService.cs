using System.Security.Cryptography;
using System.Text;
using FlowFeedback.Application.Interfaces;
using FlowFeedback.Domain.Interfaces;

namespace FlowFeedback.Application.Services;

public class DeviceService(IDeviceMasterRepository deviceMasterRepository) : IDeviceService
{
    public async Task<string> RegistrarNovoDispositivoAsync(Guid tenantId, string nomeDispositivo)
    {
        var rawKey = "dev_" + GenerateRandomToken(32);

        var keyHash = ComputeSha256Hash(rawKey);

        await deviceMasterRepository.RegistrarNovoDispositivoAsync(tenantId, nomeDispositivo, keyHash);

        return rawKey;
    }

    private static string GenerateRandomToken(int length)
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[length];
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes)
            .Replace("+", "").Replace("/", "").Replace("=", "")[..length];
    }

    public static string ComputeSha256Hash(string rawData)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
        return Convert.ToBase64String(bytes);
    }
}