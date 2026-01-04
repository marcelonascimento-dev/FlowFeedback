using FlowFeedback.Domain.Entities;
using Microsoft.EntityFrameworkCore; // <--- O pacote está aqui

namespace FlowFeedback.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<AlvoAvaliacao> AlvosAvaliacao { get; set; }
    public DbSet<Voto> Votos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}