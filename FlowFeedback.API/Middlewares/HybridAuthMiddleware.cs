using FlowFeedback.Domain.Interfaces;

namespace FlowFeedback.API.Middlewares;

public class HybridAuthMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IDeviceMasterRepository deviceMasterRepository)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            await next(context);
            return;
        }

        if (context.Request.Headers.TryGetValue("X-Device-Key", out var extractedApiKey))
        {
            await HandleDeviceAuth(context, deviceMasterRepository, extractedApiKey.ToString());
            return;
        }

        await next(context);
    }

    private async Task HandleDeviceAuth(HttpContext context, IDeviceMasterRepository deviceMasterRepository, string apiKey)
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

        if (string.IsNullOrEmpty(licenca.HardwareSignature))
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