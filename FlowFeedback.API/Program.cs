using System.Text;
using FlowFeedback.API.Consumers;
using FlowFeedback.API.Endpoints;
using FlowFeedback.API.Middlewares;
using FlowFeedback.Application.DTOs;
using FlowFeedback.Application.Interfaces;
using FlowFeedback.Application.Interfaces.Security;
using FlowFeedback.Application.Services;
using FlowFeedback.Domain.Interfaces;
using FlowFeedback.Infrastructure.Contexts;
using FlowFeedback.Infrastructure.Data;
using FlowFeedback.Infrastructure.Repositories;
using FlowFeedback.Infrastructure.Security;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();


// ==========================
// INFRA / CORE
// ==========================

builder.Services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddScoped<ITenantContext, TenantContext>();
builder.Services.AddHttpContextAccessor();



// ==========================
// CACHE
// ==========================

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


// ==========================
// SECURITY
// ==========================

var masterKey = builder.Configuration["Crypto:MasterKey"]
    ?? throw new InvalidOperationException("Crypto:MasterKey não configurada.");

builder.Services.AddSingleton<ICryptoService>(_ => new CryptoService(masterKey));

// Vault / Secrets → Scoped
builder.Services.AddScoped<ISecretProvider, AzureKeyVaultSecretProvider>();


// ==========================
// REPOSITORIES
// ==========================

builder.Services.AddScoped<IVotoRepository, VotoRepository>();
builder.Services.AddScoped<ICadastroRepository, CadastroRepository>();
builder.Services.AddScoped<IAnalyticsRepository, AnalyticsRepository>();
builder.Services.AddScoped<IDeviceMasterRepository, DeviceMasterRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITenantRepository, TenantRepository>();
builder.Services.AddScoped<IUserTenantRepository, UserTenantRepository>();


// ==========================
// DISPOSITIVOS (CACHE DECORATOR)
// ==========================

builder.Services.AddScoped<DispositivoRepository>();

builder.Services.AddScoped<IDispositivoRepository>(provider =>
    new CachedDispositivoRepository(
        provider.GetRequiredService<DispositivoRepository>(),
        provider.GetRequiredService<IDistributedCache>()
    ));


// ==========================
// MASS TRANSIT / RABBITMQ
// ==========================

var useQueue = builder.Configuration.GetValue<bool>("Messaging:UseQueue");

if (useQueue)
{
    builder.Services.AddMassTransit(x =>
    {
        x.AddConsumer<ProcessarVotosConsumer>();

        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(builder.Configuration.GetConnectionString("RabbitMq"));

            cfg.ConfigureEndpoints(context);
        });
    });
}

// ==========================
// APPLICATION SERVICES
// ==========================

builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDeviceService, DeviceService>();
builder.Services.AddScoped<ITenantService, TenantService>();
builder.Services.AddScoped<ISetupService, SetupService>();

var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSettingsSection);

var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});


// ==========================
// APP
// ==========================

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


app.UseMiddleware<TenantIdentifierMiddleware>();
app.UseAuthentication();
app.UseMiddleware<HybridAuthMiddleware>();


// ==========================
// ENDPOINTS
// ==========================

app.MapAuthEndpoints();
app.MapCadastroEndpoints();
app.MapSyncEndpoints();
app.MapSetupEndpoints();

app.Run();
