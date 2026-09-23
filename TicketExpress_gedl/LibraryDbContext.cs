using Microsoft.EntityFrameworkCore;
using TicketExpress_gedl.Models;

namespace TicketExpress_gedl;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

    public DbSet<Evento> Eventos => Set<Evento>();
    public DbSet<Boleto> Boletos => Set<Boleto>();
}