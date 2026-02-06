namespace FlowFeedback.API.Middlewares;

public class TenantIdentifierMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var tenantClaim = context.User.FindFirst("tenant_id")?.Value;

            if (!string.IsNullOrEmpty(tenantClaim) && Guid.TryParse(tenantClaim, out Guid tenantId))
            {
                context.Items["TenantId"] = tenantId;
            }
        }

        await next(context);
    }
}