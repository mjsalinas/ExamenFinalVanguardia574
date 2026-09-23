using Microsoft.EntityFrameworkCore;
using TicketExpress.Models;

namespace TicketExpress;
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options): base(options){}
        public DbSet<Evento> Evento => Set<Evento>();
        public DbSet<Boleto> Boleto => Set<Boleto>();

    }