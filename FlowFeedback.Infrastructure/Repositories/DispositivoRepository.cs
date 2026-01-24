using FlowFeedback.Domain.Entities;
using FlowFeedback.Domain.Interfaces;
using FlowFeedback.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FlowFeedback.Infrastructure.Repositories;

public class DispositivoRepository(AppDbContext context) : IDispositivoRepository
{
    public async Task<Dispositivo?> GetByIdentifierAsync(string deviceIdentifier)
    {
        return await context.Dispositivos
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == deviceIdentifier && d.Ativo);
    }
}