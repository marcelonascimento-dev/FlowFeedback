using FlowFeedback.Domain.Entities;

namespace FlowFeedback.Domain.Interfaces
{
    public interface ITenantRepository
    {
        Task<Tenant> CadastrarTenantAsync(Tenant tenant);
        Task<Tenant?> GetTenantAsync(Guid tenantId);
        Task<Tenant?> GetTenantAsync(string slug);
    }
}
