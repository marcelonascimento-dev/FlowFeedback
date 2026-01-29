using FlowFeedback.Domain.Entities;

namespace FlowFeedback.Domain.Interfaces;

public interface ICadastroRepository
{
    Task<Tenant> AddTenantAsync(Tenant tenant);
    Task<Tenant?> GetTenantByIdAsync(Guid id);
    Task<Empresa> AddEmpresaAsync(Empresa Empresa);
    Task<Empresa?> GetEmpresaByIdAsync(Guid id);
    Task<AlvoAvaliacao> AddAlvoAsync(AlvoAvaliacao alvo);
    Task<AlvoAvaliacao?> GetAlvoByIdAsync(Guid id);
}