using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExamenFinalVanguardia574  .Models;

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

    [HttpPost]
    public async Task<IActionResult> Create(Boleto autor)
    {
        // Transformación de datos (futuro AppService)
        AplicarTransformacion(Boleto);

        // Lógica de negocio mezclada directamente en el controlador (a propósito)
        if (string.IsNullOrWhiteSpace(autor.Nombre))
            return BadRequest("El nombre del autor es obligatorio.");

        var errorValidacion = ValidarAutor(Boleto);
        if (errorValidacion is not null)
            return BadRequest(errorValidacion);

        // Regla de negocio (futuro Domain Service): no registrar autores duplicados.
        var duplicado = await ExisteAutorDuplicadoAsync(autor.Nombre, autor.Nacionalidad);
        if (duplicado)
            return Conflict("Ya existe un autor con el mismo nombre y nacionalidad.");

        _db.Autores.Add(autor);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), new { id = autor.Id }, autor);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Autor autorActualizado)
    {
        if (id <= 0)
            return BadRequest("El identificador del autor debe ser mayor que cero.");

        var autor = await _db.Autores.FindAsync(id);
        if (autor is null) return NotFound();

        // Transformación de datos (futuro AppService)
        AplicarTransformacion(autorActualizado);

        // Validaciones lógicas (futuro Domain Service)
        if (string.IsNullOrWhiteSpace(autorActualizado.Nombre))
            return BadRequest("El nombre del autor es obligatorio.");

        var errorValidacion = ValidarAutor(autorActualizado);
        if (errorValidacion is not null)
            return BadRequest(errorValidacion);

        // Regla de negocio (futuro Domain Service): unicidad excluyendo el propio registro.
        var duplicado = await ExisteAutorDuplicadoAsync(
            autorActualizado.Nombre,
            autorActualizado.Nacionalidad,
            id);
        if (duplicado)
            return Conflict("Ya existe un autor con el mismo nombre y nacionalidad.");

        autor.Nombre = autorActualizado.Nombre;
        autor.Nacionalidad = autorActualizado.Nacionalidad;

        await _db.SaveChangesAsync();
        return Ok(autor);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
            return BadRequest("El identificador del autor debe ser mayor que cero.");

        var autor = await _db.Autores.FindAsync(id);
        if (autor is null) return NotFound();

        // Regla de negocio (futuro Domain Service):
        // un autor con libros en el catálogo no puede eliminarse (integridad de dominio).
        var tieneLibros = await _db.Libros.AnyAsync(l => l.AutorId == id);
        if (tieneLibros)
            return Conflict("No se puede eliminar un autor que tiene libros asociados.");

        _db.Autores.Remove(autor);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // --- Transformación de datos: se extraerá a AutorAppService ---

    private static void AplicarTransformacion(Autor autor)
    {
        autor.Nombre = NormalizarNombrePropio(autor.Nombre);
        autor.Nacionalidad = NormalizarNombrePropio(autor.Nacionalidad);
    }

    private static string NormalizarNombrePropio(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            return string.Empty;

        var colapsado = Regex.Replace(valor.Trim(), @"\s+", " ");
        var cultura = CultureInfo.GetCultureInfo("es-HN");
        return cultura.TextInfo.ToTitleCase(colapsado.ToLower(cultura));
    }

    // --- Validaciones lógicas: se extraerán a AutorDomainService ---

    private static string? ValidarAutor(Autor autor)
    {
        if (autor.Nombre.Length < NombreMinLength)
            return $"El nombre debe tener al menos {NombreMinLength} caracteres.";

        if (autor.Nombre.Length > NombreMaxLength)
            return $"El nombre no puede superar los {NombreMaxLength} caracteres.";

        if (!NombreValidoRegex.IsMatch(autor.Nombre))
            return "El nombre solo puede contener letras, espacios, guiones o apóstrofos.";

        if (string.IsNullOrWhiteSpace(autor.Nacionalidad))
            return "La nacionalidad del autor es obligatoria.";

        if (autor.Nacionalidad.Length < NacionalidadMinLength)
            return $"La nacionalidad debe tener al menos {NacionalidadMinLength} caracteres.";

        if (autor.Nacionalidad.Length > NacionalidadMaxLength)
            return $"La nacionalidad no puede superar los {NacionalidadMaxLength} caracteres.";

        if (!NombreValidoRegex.IsMatch(autor.Nacionalidad))
            return "La nacionalidad solo puede contener letras, espacios, guiones o apóstrofos.";

        return null;
    }

    // --- Reglas de negocio: se extraerán a AutorDomainService ---

    private async Task<bool> ExisteAutorDuplicadoAsync(
        string nombre,
        string nacionalidad,
        int? excluirId = null)
    {
        var query = _db.Autores.Where(a =>
            a.Nombre.ToLower() == nombre.ToLower() &&
            a.Nacionalidad.ToLower() == nacionalidad.ToLower());

        if (excluirId.HasValue)
            query = query.Where(a => a.Id != excluirId.Value);

        return await query.AnyAsync();
    }
}