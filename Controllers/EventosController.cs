using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExamenFinalVanguardia574.Data;
using ExamenFinalVanguardia574.Model;

namespace ExamenFinalVanguardia574.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventosController : ControllerBase

{
    private readonly LibraryDbContext _db;

    public EventosController(LibraryDbContext db) => _db = db;

 [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var eventos = await _db.Eventos.ToListAsync();
        return Ok(eventos);
    }

[HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var evento = await _db.Eventos.FindAsync(id);
        if (evento == null)
        {
            return NotFound();
        }
        return Ok(evento);
    }
[HttpPost]
public async Task<IActionResult> Crear(Evento evento)
{
    // Validar Nombre
    if (string.IsNullOrWhiteSpace(evento.Nombre) ||
        evento.Nombre.Length < 3 ||
        evento.Nombre.Length > 100)
    {
        return BadRequest("Nombre: obligatorio, entre 3 y 100 caracteres.");
    }

    // Validar Ciudad
    if (string.IsNullOrWhiteSpace(evento.Ciudad) ||
        evento.Ciudad.Length < 2 ||
        evento.Ciudad.Length > 60 ||
        !Regex.IsMatch(evento.Ciudad, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s'-]+$"))
    {
        return BadRequest("Ciudad: obligatoria, entre 2 y 60 caracteres, solo letras, espacios, guiones o apóstrofes.");
    }

    // Validar Fecha
    if (evento.Fecha.Date < DateTime.Today)
    {
        return BadRequest("Fecha: obligatoria, debe ser hoy o una fecha futura.");
    }

    // Validar CapacidadTotal
    if (evento.CapacidadTotal <= 0)
    {
        return BadRequest("CapacidadTotal: debe ser un entero mayor a 0.");
    }

    // Validar PrecioBoleto
    if (evento.PrecioBoleto < 0)
    {
        return BadRequest("PrecioBoleto: no puede ser negativo.");
    }

    _db.Eventos.Add(evento);
    await _db.SaveChangesAsync();

    return Ok(evento);
}
}




