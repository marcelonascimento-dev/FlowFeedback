using FlowFeedback.Domain.Interfaces;

namespace FlowFeedback.API.Middlewares;

public class HybridAuthMiddleware(RequestDelegate next, IDeviceMasterRepository deviceMasterRepository)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            await next(context);
            return;
        }

        if (context.Request.Headers.TryGetValue("X-Device-Key", out var extractedApiKey))
        {
            await HandleDeviceAuth(context, extractedApiKey.ToString());
            return;
        }

        await next(context);
    }

    private async Task HandleDeviceAuth(HttpContext context, string apiKey)
    {
        if (!context.Request.Headers.TryGetValue("X-Hardware-Sig", out var currentHardwareSig))
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Hardware Signature obrigatoria.");
            return;
        }

        var licenca = await deviceMasterRepository.ObterLicencaPorChaveAsync(apiKey);

        if (licenca == null)
        {
            context.Response.StatusCode = 401;
            return;
        }

        if (licenca.HardwareSignature == null)
        {
            await deviceMasterRepository.VincularHardwareAsync(apiKey, currentHardwareSig.ToString());
        }
        else if (licenca.HardwareSignature != currentHardwareSig.ToString())
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Licenca em uso por outro dispositivo.");
            return;
        }

        context.Request.Headers["X-Tenant-Code"] = licenca.TenantCode.ToString();
        await next(context);
    }
}