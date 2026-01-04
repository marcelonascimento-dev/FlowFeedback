using FlowFeedback.Application.Interfaces;
using FlowFeedback.Application.Services;
using FlowFeedback.Domain.Interfaces;
using FlowFeedback.Infrastructure.Data;
using FlowFeedback.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IVotoRepository, VotoRepository>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();