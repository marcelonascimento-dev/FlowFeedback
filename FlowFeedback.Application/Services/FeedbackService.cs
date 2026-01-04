using FlowFeedback.Application.DTOs;
using FlowFeedback.Application.Interfaces;
using FlowFeedback.Domain.Entities;
using FlowFeedback.Domain.Interfaces;
using FlowFeedback.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FlowFeedback.Application.Services;

public class FeedbackService(IVotoRepository votoRepository, AppDbContext context) : IFeedbackService
{

    public async Task ProcessarVotosDoTabletAsync(Guid tenantId, Guid deviceId, List<RegistrarVotoDto> votosDto)
    {
        var dispositivo = await context.Dispositivos
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == deviceId && d.TenantId == tenantId);

        if (dispositivo == null)
        {
            return;
        }

        var entidades = new List<Voto>();

        foreach (var dto in votosDto)
        {
            var voto = new Voto(
                tenantId,
                dispositivo.UnidadeId,
                deviceId,
                dto.AlvoAvaliacaoId,
                dto.Nota,
                dto.DataHora,
                dto.TagMotivo
            );
            entidades.Add(voto);
        }

        await votoRepository.AdicionarLoteAsync(entidades);
    }
}