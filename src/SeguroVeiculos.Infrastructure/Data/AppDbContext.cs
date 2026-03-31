using Microsoft.EntityFrameworkCore;
using SeguroVeiculos.Domain.Entities;
using SeguroVeiculos.Domain.ValueObjects;

namespace SeguroVeiculos.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Seguro> Seguros => Set<Seguro>();
    public DbSet<Veiculo> Veiculos => Set<Veiculo>();
    public DbSet<Segurado> Segurados => Set<Segurado>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Veiculo>(entity =>
        {
            entity.HasKey(v => v.Id);
            entity.Property(v => v.Valor).HasColumnType("numeric(18,2)").IsRequired();
            entity.Property(v => v.MarcaModelo).HasMaxLength(200).IsRequired();
        });

        modelBuilder.Entity<Segurado>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Nome).HasMaxLength(200).IsRequired();
            entity.Property(s => s.Cpf).HasMaxLength(14).IsRequired();
            entity.Property(s => s.Idade).IsRequired();
        });

        modelBuilder.Entity<Seguro>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.HasOne(s => s.Veiculo).WithMany().HasForeignKey("VeiculoId");
            entity.HasOne(s => s.Segurado).WithMany().HasForeignKey("SeguradoId");
            entity.Property(s => s.CriadoEm).IsRequired();

            // Owned type para CalculoSeguro
            entity.OwnsOne(s => s.Calculo, calculo =>
            {
                calculo.Property(c => c.ValorVeiculo).HasColumnName("Calculo_ValorVeiculo").HasColumnType("numeric(18,2)");
                calculo.Property(c => c.TaxaRisco).HasColumnName("Calculo_TaxaRisco").HasColumnType("numeric(18,4)");
                calculo.Property(c => c.PremioRisco).HasColumnName("Calculo_PremioRisco").HasColumnType("numeric(18,2)");
                calculo.Property(c => c.PremioPuro).HasColumnName("Calculo_PremioPuro").HasColumnType("numeric(18,2)");
                calculo.Property(c => c.PremioComercial).HasColumnName("Calculo_PremioComercial").HasColumnType("numeric(18,2)");
            });
        });
    }
}

