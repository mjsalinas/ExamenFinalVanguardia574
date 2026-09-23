using Microsoft.EntityFrameworkCore;
using ExamenFinalVanguardia574.Model;


namespace ExamenFinalVanguardia574.Data;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
        : base(options)
    {
    }

    public DbSet<Boleto> Boletos { get; set; }

    public DbSet<Evento> Eventos { get; set; }
}
