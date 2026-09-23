using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExamenFinalVanguardia574.Data;
using ExamenFinalVanguardia574.Model;

namespace ExamenFinalVanguardia574.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BoletosController : ControllerBase
{
    private readonly LibraryDbContext _db;

    public BoletosController(LibraryDbContext db) => _db = db;
    
[HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var boletos = await _db.Boletos.ToListAsync();
        return Ok(boletos);
    }

[HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var boleto = await _db.Boletos.FindAsync(id);
        if (boleto == null)
        {
            return NotFound();
        }
        return Ok(boleto);
    }
[HttpPost]
public async Task<IActionResult> Crear(Boleto boleto)
{
    if (string.IsNullOrWhiteSpace(boleto.NombreComprador) ||
        boleto.NombreComprador.Length < 2 ||
        boleto.NombreComprador.Length > 100 ||
        !Regex.IsMatch(boleto.NombreComprador, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s'-]+$"))
    {
        return BadRequest("NombreComprador: obligatorio, entre 2 y 100 caracteres, solo letras, espacios, guiones o apóstrofes.");
    }

    if (string.IsNullOrWhiteSpace(boleto.CorreoComprador) ||
        !new EmailAddressAttribute().IsValid(boleto.CorreoComprador))
    {
        return BadRequest("CorreoComprador: obligatorio y debe tener un formato de correo válido.");
    }

    if (boleto.Cantidad <= 0)
    {
        return BadRequest("Cantidad: obligatorio, debe ser un entero mayor a 0.");
    }

    boleto.FechaCompra = DateTime.Now;

    _db.Boletos.Add(boleto);
    await _db.SaveChangesAsync();

    return Ok(boleto);
}
}
