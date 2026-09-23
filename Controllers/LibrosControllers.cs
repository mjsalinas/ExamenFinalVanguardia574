using System.Globalization;
using System.Text.RegularExpressions;
using ExamenFinalVanguardia574.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExamenFinalVanguardia574.Controllers;


[ApiController]
[Route("api/[controller]")]
public class LibrosController : ControllerBase
{
    // Gutenberg ~1440: primer año razonable para un libro impreso.
    private const int AnioMinimoPublicacion = 1440;
    private const int TituloMinLength = 3;
    private const int TituloMaxLength = 200;

    private readonly LibraryDbContext _db;

   public LibrosController(LibraryDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var libros = await _db.Libros.Include(l => l.Autor).ToListAsync();

        // Transformación de lectura (futuro AppService):
        // ordenar el catálogo para consumo consistente de la API.
        var catalogo = libros
            .OrderBy(l => l.Titulo, StringComparer.CurrentCultureIgnoreCase)
            .ThenBy(l => l.AnioPublicacion)
            .ToList();

        return Ok(catalogo);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0)
            return BadRequest("El identificador del libro debe ser mayor que cero.");

        var libro = await _db.Libros.Include(l => l.Autor)
            .FirstOrDefaultAsync(l => l.Id == id);
        if (libro is null) return NotFound();
        return Ok(libro);
    }


}
