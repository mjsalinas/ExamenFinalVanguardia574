using Microsoft.EntityFrameworkCore;
using ExamenFinalVanguardia574.Models;

namespace ExamenFinalVanguardia574.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Evento> Eventos { get; set; }

    public DbSet<Boleto> Boletos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Boleto>()
            .HasOne(b => b.Evento)
            .WithMany(e => e.Boletos)
            .HasForeignKey(b => b.EventoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}