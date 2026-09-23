using Microsoft.EntityFrameworkCore;
using TicketExpress.Models;

namespace TicketExpress;

public class TicketExpressDbContext : DbContext
{
    public TicketExpressDbContext(DbContextOptions<TicketExpressDbContext> options) : base(options) { }

    public DbSet<Evento> Eventos => Set<Evento>();
    public DbSet<Boleto> Boletos => Set<Boleto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Boleto>()
            .HasOne(b => b.Evento)
            .WithMany(e => e.Boletos)
            .HasForeignKey(b => b.EventoId);

        modelBuilder.Entity<Evento>()
            .Property(e => e.PrecioBoleto)
            .HasColumnType("decimal(18,2)");
    }
}