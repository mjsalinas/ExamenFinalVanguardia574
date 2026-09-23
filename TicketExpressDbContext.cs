using Microsoft.EntityFrameworkCore;
using TicketExpress.Models;

namespace TicketExpress;

public class TicketExpressDbContext : DbContext
{
    public TicketExpressDbContext(
        DbContextOptions<TicketExpressDbContext> options) : base(options)
    {
    }

    public DbSet<Evento> Eventos { get; set; } = null!;
    public DbSet<Boleto> Boletos { get; set; } = null!;
}