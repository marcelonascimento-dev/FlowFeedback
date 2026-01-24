using FlowFeedback.API.Consumers;
using FlowFeedback.Application.Interfaces;
using FlowFeedback.Application.Services;
using FlowFeedback.Domain.Interfaces;
using FlowFeedback.Infrastructure.Data;
using FlowFeedback.Infrastructure.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<ProcessarVotosConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });
});

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

builder.Services.AddScoped<IVotoRepository, VotoRepository>();
builder.Services.AddScoped<DispositivoRepository>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<ICadastroService, CadastroService>();

builder.Services.AddScoped<IDispositivoRepository>(provider =>
    new CachedDispositivoRepository(
        provider.GetRequiredService<DispositivoRepository>(),
        provider.GetRequiredService<IDistributedCache>()
    ));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();