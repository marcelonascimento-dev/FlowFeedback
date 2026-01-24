using FlowFeedback.Domain.Entities;
using FlowFeedback.Domain.Interfaces;
using FlowFeedback.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FlowFeedback.Infrastructure.Repositories;

public class VotoRepository : IVotoRepository
{
    private readonly AppDbContext _context;

    public VotoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(Voto voto)
    {
        await _context.Votos.AddAsync(voto);
        await _context.SaveChangesAsync();
    }

    public async Task AdicionarLoteAsync(IEnumerable<Voto> votos)
    {
        await _context.Votos.AddRangeAsync(votos);
        await _context.SaveChangesAsync();
    }

    public Task<IEnumerable<Voto>> ObterPorTenantAsync(Guid tenantId, DateTime dataInicio, DateTime dataFim)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Votos
            .AnyAsync(v => v.Id == id);
    }

    public async Task<HashSet<Guid>> GetExistingIdsAsync(IEnumerable<Guid> idsToCheck)
    {
        var existing = await _context.Votos
            .AsNoTracking()
            .Where(v => idsToCheck.Contains(v.Id))
            .Select(v => v.Id)
            .ToListAsync();

        return [.. existing];
    }
}