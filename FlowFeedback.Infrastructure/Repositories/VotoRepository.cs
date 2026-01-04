using FlowFeedback.Domain.Entities;
using FlowFeedback.Domain.Interfaces;

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
}