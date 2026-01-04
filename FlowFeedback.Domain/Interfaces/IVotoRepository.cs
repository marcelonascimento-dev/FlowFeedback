using FlowFeedback.Domain.Entities;

namespace FlowFeedback.Domain.Interfaces;

public interface IVotoRepository
{
    Task AdicionarAsync(Voto voto);
    Task AdicionarLoteAsync(IEnumerable<Voto> votos);
    Task<IEnumerable<Voto>> ObterPorTenantAsync(Guid tenantId, DateTime dataInicio, DateTime dataFim);
}