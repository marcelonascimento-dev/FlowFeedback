using System;
using System.Collections.Generic;
using System.Text;
using FlowFeedback.Domain.Entities;

namespace FlowFeedback.Domain.Interfaces
{
    public interface IUserTenantRepository
    {
        Task<UserTenant?> GetByEmailAsync(string email);
        Task<IEnumerable<UserTenant>> GetByUserIdAsync(Guid userId);
        Task<UserTenant?> GetByIdsAsync(Guid userId, Guid tenantId);
        Task<UserTenant> CadastrarAsync(UserTenant userTenant);
    }

}
