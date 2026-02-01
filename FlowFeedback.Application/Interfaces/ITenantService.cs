using FlowFeedback.Application.DTOs;
using FlowFeedback.Application.Models;
using FlowFeedback.Domain.Models;

namespace FlowFeedback.Application.Interfaces
{
    public interface ITenantService
    {
        Task<(Guid, DateTime)> CadastrarTenantAsync(TenantCreateDto dto);
        Task<TenantResponseDto> GetTenantAsync(Guid tenantId);
        Task<TenantResponseDto> GetTenantAsync(string slug);
    }
}
