using FlowFeedback.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FlowFeedback.Infrastructure.Contexts;

public class TenantContext(IHttpContextAccessor accessor) : ITenantContext
{
    public int TenantCode
    {
        get
        {
            var claimValue = accessor.HttpContext?.User?.FindFirst("TenantCode")?.Value;

            if (int.TryParse(claimValue, out var tenantCode))
            {
                return tenantCode;
            }

            return 0;
        }
    }
}