using FlowFeedback.Application.DTOs;
using FlowFeedback.Application.Interfaces;
using FlowFeedback.Domain.Entities;
using FlowFeedback.Domain.Interfaces;

namespace FlowFeedback.Application.Services;

public class FeedbackService : IFeedbackService
{
    private readonly IVotoRepository _votoRepository;

    public FeedbackService(IVotoRepository votoRepository)
    {
        _votoRepository = votoRepository;
    }

    public async Task ProcessarVotosDoTabletAsync(Guid tenantId, Guid deviceId, List<RegistrarVotoDto> votosDto)
    {
        var entidades = new List<Voto>();

        foreach (var dto in votosDto)
        {
            var voto = new Voto(tenantId, deviceId, dto.FuncionarioId, dto.Nota, dto.DataHora);
            entidades.Add(voto);
        }

        await _votoRepository.AdicionarLoteAsync(entidades);
    }
}