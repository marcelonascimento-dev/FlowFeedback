using FlowFeedback.Domain.Entities;

namespace FlowFeedback.Domain.Interfaces;

public interface IAlvoAvaliacaoRepository
{
    Task<IEnumerable<AlvoAvaliacao>> GetAllAsync();
    Task<AlvoAvaliacao?> GetByIdAsync(Guid id);
    Task AddAsync(AlvoAvaliacao alvo);
    Task UpdateAsync(AlvoAvaliacao alvo);
    Task DeleteAsync(Guid id);
}
