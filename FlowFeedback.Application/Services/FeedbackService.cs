using FlowFeedback.Application.DTOs;
using FlowFeedback.Application.Interfaces;
using FlowFeedback.Application.Mappings;
using FlowFeedback.Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace FlowFeedback.Application.Services;

public class FeedbackService(
    IDispositivoRepository dispositivoRepository,
    IVotoRepository votoRepository,
    IDistributedCache cache,
    ILogger<FeedbackService> logger) : IFeedbackService
{
    public async Task ProcessarPacoteVotos(PacoteVotosDto pacote)
    {
        var dispositivo = await dispositivoRepository.GetByIdentifierAsync(pacote.DeviceId);

        if (dispositivo == null || !dispositivo.Ativo)
        {
            logger.LogWarning("Dispositivo desconhecido ou inativo: {Identifier}", pacote.DeviceId);
            return;
        }

        var votosNaoProcessadosRecentemente = new List<RegistrarVotoDto>();

        foreach (var voto in pacote.Votos)
        {
            var cacheKey = $"voto_proc:{voto.Id}";
            var processado = await cache.GetAsync(cacheKey);
            if (processado == null)
                votosNaoProcessadosRecentemente.Add(voto);
        }

        if (votosNaoProcessadosRecentemente.Count == 0)
        {
            logger.LogInformation("Pacote ignorado: Todos os votos já foram processados recentemente (Cache Hit).");
            return;
        }

        var idsParaChecar = votosNaoProcessadosRecentemente.Select(v => v.Id).ToList();
        var idsExistentesNoBanco = await votoRepository.GetExistingIdsAsync(idsParaChecar);

        var votosParaSalvar = votosNaoProcessadosRecentemente
            .Where(dto => !idsExistentesNoBanco.Contains(dto.Id))
            .Select(dto => dto.ToEntity(dispositivo))
            .ToList();

        if (votosParaSalvar.Any())
        {
            await votoRepository.AdicionarLoteAsync(votosParaSalvar);

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
            };

            var tasksCache = votosParaSalvar.Select(v =>
                cache.SetStringAsync($"voto_proc:{v.Id}", "1", cacheOptions));

            await Task.WhenAll(tasksCache);

            logger.LogInformation("{Qtd} novos votos inseridos e cacheados.", votosParaSalvar.Count);
        }
    }
}