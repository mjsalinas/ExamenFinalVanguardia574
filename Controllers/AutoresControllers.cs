using System.Globalization;
using System.Text.RegularExpressions;
using ExamenFinalVanguardia574.Data;
using ExamenFinalVanguardia574.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExamenFinalVanguardia574.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AutoresController : ControllerBase
{
    private const int NombreMinLength = 2;
    private const int NombreMaxLength = 100;
    private const int NacionalidadMinLength = 3;
    private const int NacionalidadMaxLength = 60;

    private static readonly Regex NombreValidoRegex =
        new(@"^[\p{L}\s'\-\.]+$", RegexOptions.Compiled);

    private readonly LibraryDbContext _db;
   public AutoresController(LibraryDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var autores = await _db.Autores.ToListAsync();

        // Transformación de lectura (futuro AppService): catálogo ordenado por nombre.
        var catalogo = autores
            .OrderBy(a => a.Nombre, StringComparer.CurrentCultureIgnoreCase)
            .ThenBy(a => a.Nacionalidad, StringComparer.CurrentCultureIgnoreCase)
            .ToList();

        return Ok(catalogo);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0)
            return BadRequest("El identificador del autor debe ser mayor que cero.");

        var autor = await _db.Autores.FirstOrDefaultAsync(a => a.Id == id);
        if (autor is null) return NotFound();
        return Ok(autor);
    }





}