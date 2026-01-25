using FlowFeedback.Domain.Entities;

namespace FlowFeedback.Domain.Interfaces;

public interface IVotoRepository
{
    Task<HashSet<Guid>> GetExistingIdsAsync(IEnumerable<Guid> idsToCheck);
    Task AdicionarLoteAsync(IEnumerable<Voto> votos);
}