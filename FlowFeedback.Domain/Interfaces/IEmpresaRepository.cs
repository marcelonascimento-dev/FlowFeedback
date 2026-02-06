using FlowFeedback.Domain.Entities;

namespace FlowFeedback.Domain.Interfaces;

public interface IEmpresaRepository
{
    Task<IEnumerable<Empresa>> GetAllAsync();
    Task<Empresa?> GetByIdAsync(Guid id);
    Task AddAsync(Empresa empresa);
    Task UpdateAsync(Empresa empresa);
    Task DeleteAsync(Guid id);
}
