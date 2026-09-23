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

    //private static readonly Regex NombreValidoRegex =
    //    new(@"^[\p{L}\s'\-\.]+$", RegexOptions.Compiled);

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


}
