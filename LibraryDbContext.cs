using Microsoft.EntityFrameworkCore;
using ExamenFinalVanguardia574.Models;

namespace ExamenFinalVanguardia574;

public class LibraryDbContext: DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options): base(options){}

    public DbSet<Autor> Autores => Set<Autor>();
    public DbSet<Libro> Libros => Set<Libro>();
}