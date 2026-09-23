using Microsoft.EntityFrameworkCore;
using ExamenFinalVanguardia574.Models;


namespace ExamenFinalVanguardia574;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options){}

    public DbSet<Boleto> Boletos => Set<Boleto>();
    public DbSet<Evento> Eventos => Set<Evento>();
}