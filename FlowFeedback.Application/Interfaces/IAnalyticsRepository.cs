using FlowFeedback.Application.DTOs;
using FlowFeedback.Domain.Models;

namespace FlowFeedback.Application.Interfaces;

public interface IAnalyticsRepository
{
    Task<NpsScoreDto> GetNpsPeriodoAsync(Guid tenantId, DateTime inicio, DateTime fim);
    Task<IEnumerable<NpsEvolucaoDiariaDto>> GetEvolucaoNpsAsync(Guid tenantId, DateTime inicio, DateTime fim);
}