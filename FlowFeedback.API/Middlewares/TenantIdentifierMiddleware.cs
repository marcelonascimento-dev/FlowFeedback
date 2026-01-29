namespace FlowFeedback.API.Middlewares;

public class TenantIdentifierMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var tenantCodeClaim = context.User.FindFirst("TenantCode")?.Value;

            if (!string.IsNullOrEmpty(tenantCodeClaim) && int.TryParse(tenantCodeClaim, out int tenantCode))
            {
                context.Request.Headers["X-Tenant-Code"] = tenantCode.ToString();

                context.Items["TenantCode"] = tenantCode;
            }
        }

        await next(context);
    }
}