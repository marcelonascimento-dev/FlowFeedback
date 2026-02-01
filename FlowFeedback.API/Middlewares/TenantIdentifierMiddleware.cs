namespace FlowFeedback.API.Middlewares;

public class TenantIdentifierMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var tenantClaim = context.User.FindFirst("TenantId")?.Value;

            if (!string.IsNullOrEmpty(tenantClaim) && Guid.TryParse(tenantClaim, out Guid tenantId))
            {
                // context.Request.Headers["X-Tenant-Code"] = tenantCode.ToString(); // Possibly remove or update header name
                // Keeping header for backward compatibility if needed, but changing value?
                // Request didn't specify header standard. Let's assume we can change it to X-Tenant-Id or keep logic consistent with internal usage.

                context.Items["TenantId"] = tenantId;
            }
        }

        await next(context);
    }
}