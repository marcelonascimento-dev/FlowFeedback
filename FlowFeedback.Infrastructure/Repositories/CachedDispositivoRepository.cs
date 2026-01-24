using System.Text.Json;
using FlowFeedback.Domain.Entities;
using FlowFeedback.Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace FlowFeedback.Infrastructure.Repositories;

public class CachedDispositivoRepository : IDispositivoRepository
{
    private readonly IDispositivoRepository _innerRepository;
    private readonly IDistributedCache _cache;
    private readonly DistributedCacheEntryOptions _cacheOptions;

    public CachedDispositivoRepository(IDispositivoRepository innerRepository, IDistributedCache cache)
    {
        _innerRepository = innerRepository;
        _cache = cache;

        _cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
            SlidingExpiration = TimeSpan.FromMinutes(2)
        };
    }

    public async Task<Dispositivo?> GetByIdentifierAsync(string deviceIdentifier)
    {
        string cacheKey = $"device:{deviceIdentifier}";

        string? cachedData = await _cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cachedData))
        {
            return JsonSerializer.Deserialize<Dispositivo>(cachedData);
        }

        var dispositivo = await _innerRepository.GetByIdentifierAsync(deviceIdentifier);

        if (dispositivo != null)
        {
            string serializedData = JsonSerializer.Serialize(dispositivo);
            await _cache.SetStringAsync(cacheKey, serializedData, _cacheOptions);
        }

        return dispositivo;
    }
}