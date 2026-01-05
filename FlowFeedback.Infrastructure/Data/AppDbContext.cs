using FlowFeedback.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlowFeedback.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Unidade> Unidades { get; set; }
    public DbSet<AlvoAvaliacao> AlvosAvaliacao { get; set; }
    public DbSet<Voto> Votos { get; set; }
    public DbSet<Dispositivo> Dispositivos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tenant>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.NomeCorporativo).IsRequired().HasMaxLength(150);
            e.Property(x => x.Cnpj).IsRequired().HasMaxLength(18);
        });

        modelBuilder.Entity<Unidade>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.NomeLoja).IsRequired().HasMaxLength(100);
            e.HasIndex(x => x.TenantId);
        });

        modelBuilder.Entity<Dispositivo>()
            .HasMany(d => d.Alvos)
            .WithMany(a => a.Dispositivos)
            .UsingEntity(j => j.ToTable("DispositivoAlvos"));

        modelBuilder.Entity<AlvoAvaliacao>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Titulo).IsRequired().HasMaxLength(100);
            e.Property(x => x.Subtitulo).IsRequired().HasMaxLength(200);
            e.HasIndex(x => x.UnidadeId);
        });

        modelBuilder.Entity<Voto>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => new { x.TenantId, x.UnidadeId, x.DataHoraVoto });
            e.HasIndex(x => x.AlvoAvaliacaoId);
        });
    }
}