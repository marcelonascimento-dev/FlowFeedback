using System;
using System.Collections.Generic;
using System.Text;
using FlowFeedback.Domain.Entities;

namespace FlowFeedback.Domain.Interfaces
{
    public interface ITenantUserIndexRepository
    {
        Task<TenantUserIndex?> GetByEmailAsync(string email);
    }

}
