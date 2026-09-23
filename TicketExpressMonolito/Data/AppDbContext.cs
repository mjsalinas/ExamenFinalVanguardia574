using Microsoft.EntityFrameworkCore;
using TicketExpressMonolito.Models;

namespace TicketExpressMonolito.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Evento> Eventos => Set<Evento>();
    public DbSet<Boleto> Boletos => Set<Boleto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Evento>()
            .HasMany(e => e.Boletos)
            .WithOne(b => b.Evento)
            .HasForeignKey(b => b.EventoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Evento>()
            .Property(e => e.PrecioBoleto)
            .HasColumnType("decimal(18,2)");
    }
}