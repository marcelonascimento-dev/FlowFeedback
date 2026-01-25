using FlowFeedback.API.Consumers;
using FlowFeedback.API.Endpoints;
using FlowFeedback.Application.Interfaces;
using FlowFeedback.Application.Services;
using FlowFeedback.Domain.Interfaces;
using FlowFeedback.Domain.Repositories;
using FlowFeedback.Infrastructure.Data;
using FlowFeedback.Infrastructure.Repositories;
using FlowFeedback.Infrastructure.Security;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ProcessarVotosConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });
});

// --- Configuração de Cache (Redis / Memory) ---
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDistributedMemoryCache();
}
else
{
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration.GetConnectionString("Redis");
        options.InstanceName = "FlowFeedback_";
    });
}

builder.Services.AddHttpContextAccessor();
var masterKey = builder.Configuration["Crypto:MasterKey"];
builder.Services.AddSingleton<ICryptoService>(new CryptoService(masterKey));

builder.Services.AddScoped<IVotoRepository, VotoRepository>();
builder.Services.AddScoped<ICadastroRepository, CadastroRepository>();
builder.Services.AddScoped<IAnalyticsRepository, AnalyticsRepository>();
builder.Services.AddScoped<IDeviceMasterRepository, DeviceMasterRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IDeviceMasterRepository, DeviceMasterRepository>();

builder.Services.AddScoped<DispositivoRepository>();
builder.Services.AddScoped<IDispositivoRepository>(provider =>
    new CachedDispositivoRepository(
        provider.GetRequiredService<DispositivoRepository>(),
        provider.GetRequiredService<IDistributedCache>()
    ));

builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<ICadastroService, CadastroService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDeviceService, DeviceService>();  

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("FlowFeedback API")
               .WithTheme(ScalarTheme.Moon)
               .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();

// Mapeamento das Minimal APIs
app.MapAuthEndpoints();
app.MapCadastroEndpoints();
app.MapSyncEndpoints();

app.Run();