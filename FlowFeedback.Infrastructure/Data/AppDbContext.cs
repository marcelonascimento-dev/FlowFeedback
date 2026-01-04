using FlowFeedback.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlowFeedback.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<AlvoAvaliacao> AlvosAvaliacao { get; set; }
    public DbSet<Voto> Votos { get; set; }
    public DbSet<Unidade> Unidades { get; set; }
    public DbSet<Dispositivo> Dispositivos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Unidade>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.TenantId);
        });

        modelBuilder.Entity<Dispositivo>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => new { x.TenantId, x.UnidadeId });
        });

        modelBuilder.Entity<Voto>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => new { x.TenantId, x.UnidadeId, x.DataHoraVoto }); // Índice vital para relatórios por loja
        });
    }
}