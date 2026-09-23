using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;
using TicketExpress_gedl.Models;

namespace TicketExpress_gedl.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BoletosController : ControllerBase
{
    private const int NombreMinLength = 2;
    private const int NombreMaxLength = 100;
    private const int NacionalidadMinLength = 3;
    private const int NacionalidadMaxLength = 60;

    private static readonly Regex NombreValidoRegex =
        new(@"^[\p{L}\s'\-\.]+$", RegexOptions.Compiled);

    private readonly LibraryDbContext _db;

    public BoletosController(LibraryDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var boletos = await _db.Boletos.ToListAsync();

        // Transformación de lectura (futuro AppService): catálogo ordenado por nombre.
        var catalogoDeBoletos = boletos.ToList();

        return Ok(catalogoDeBoletos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0)
            return BadRequest("El identificador del boleto debe ser mayor que cero.");

        var boleto = await _db.Boletos.FirstOrDefaultAsync(b => b.Id == id);
        if (boleto is null) return NotFound();
        return Ok(boleto);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Boleto boleto)
    {

        // Lógica de negocio mezclada directamente en el controlador (a propósito)
        if (string.IsNullOrWhiteSpace(boleto.NombreComprador))
            return BadRequest("El nombre del comprador del boleto es obligatorio.");


        /*• NombreComprador: obligatorio, entre 2 y 100 caracteres, solo letras, espacios,
guiones o apóstrofes.
• CorreoComprador: obligatorio, debe tener formato de correo válido.
• Cantidad: obligatorio, entero mayor a */

       if(boleto.NombreComprador.Length < NombreMinLength || boleto.NombreComprador.Length > NombreMaxLength)
            return BadRequest($"El nombre del comprador debe tener entre {NombreMinLength} y {NombreMaxLength} caracteres.");

        /*• CorreoComprador: obligatorio, debe tener formato de correo válido.*/
        /*Debe contener el @*/
        if(string.IsNullOrWhiteSpace(boleto.CorreoComprador) || !boleto.CorreoComprador.Contains("@"))
            return BadRequest("El correo del comprador es obligatorio y debe tener un formato válido.");

    /*• Cantidad: obligatorio, entero mayor a 0.*/
        if(boleto.Cantidad <= 0)
            return BadRequest("La cantidad del boleto es obligatoria y debe ser un número mayor que cero.");



    // Regla de negocio (futuro Domain Service): no registrar boletos duplicados.
    //var duplicado = await ExisteBoletoDuplicadoAsync(boleto.Nombre, boleto.Nacionalidad);
    //if (duplicado)
    //return Conflict("Ya existe un boleto con el mismo nombre y nacionalidad.");

    _db.Boletos.Add(boleto);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), new { id = boleto.Id }, boleto);
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
