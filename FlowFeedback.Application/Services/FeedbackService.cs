using FlowFeedback.Application.DTOs;
using FlowFeedback.Application.Interfaces;
using FlowFeedback.Domain.Entities;
using FlowFeedback.Domain.Interfaces;
using FlowFeedback.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FlowFeedback.Application.Services;

public class FeedbackService : IFeedbackService
{
    private readonly IVotoRepository _votoRepository;
    private readonly AppDbContext _context;

    public FeedbackService(IVotoRepository votoRepository, AppDbContext context)
    {
        _votoRepository = votoRepository;
        _context = context;
    }

    public async Task ProcessarVotosDoTabletAsync(Guid tenantId, string deviceId, List<RegistrarVotoDto> votosDto)
    {
        var dispositivo = await _context.Dispositivos.AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == deviceId && d.TenantId == tenantId);

        if (dispositivo == null)
            throw new InvalidOperationException("Dispositivo não encontrado ou inválido para o tenant informado.");

        var entidades = votosDto.Select(dto => new Voto(
            tenantId,
            dispositivo.UnidadeId,
            deviceId,
            dto.AlvoAvaliacaoId,
            dto.Nota,
            dto.DataHora,
            dto.TagMotivo
        )).ToList();

        await _votoRepository.AdicionarLoteAsync(entidades);
    }
}