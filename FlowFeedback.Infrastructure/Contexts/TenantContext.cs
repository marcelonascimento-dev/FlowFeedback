using FlowFeedback.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FlowFeedback.Infrastructure.Contexts;

public class TenantContext(IHttpContextAccessor accessor) : ITenantContext
{
    public Guid TenantId
    {
        get
        {
            var user = accessor.HttpContext?.User;
            var claimValue = user?.FindFirst("TenantId")?.Value;

            if (Guid.TryParse(claimValue, out var tenantId))
            {
                return tenantId;
            }

            // Fallback to Header (useful for HybridAuthMiddleware)
            var headerValue = accessor.HttpContext?.Request.Headers["X-Tenant-Id"].ToString();
            if (Guid.TryParse(headerValue, out var headerTenantId))
            {
                return headerTenantId;
            }

            return Guid.Empty;
        }
    }
}