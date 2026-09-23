using Microsoft.EntityFrameworkCore;
using EventosMonolito.Models;

namespace BibliotecaMonolito;

public class LibraryDbContext: DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options): base(options){}

    public DbSet<Evento> Autores => Set<Autor>();
    public DbSet<Libro> Libros => Set<Libro>();
}