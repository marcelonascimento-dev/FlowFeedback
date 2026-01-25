using FlowFeedback.Domain.Models;

namespace FlowFeedback.Domain.Interfaces;

public interface IAnalyticsRepository
{
    Task<NpsScoreDto> GetNpsPeriodoAsync(Guid tenantId, DateTime inicio, DateTime fim);
    Task<IEnumerable<NpsEvolucaoDiariaDto>> GetEvolucaoNpsAsync(Guid tenantId, DateTime inicio, DateTime fim);
}