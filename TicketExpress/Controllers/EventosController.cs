using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketExpress.Models;
using System.Text.RegularExpressions;

namespace TicketExpress.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventosController : ControllerBase
{
    private readonly LibraryDbContext _db;

    public EventosController(LibraryDbContext db) => _db = db;

    // GET: api/Eventos
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var eventos = await _db.Eventos.ToListAsync();

        return Ok(eventos);
    }

    // GET: api/Eventos/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var evento = await _db.Eventos.FindAsync(id);

        if (evento == null)
            return NotFound("El evento no existe.");

        return Ok(evento);
    }

    // POST: api/Eventos
    [HttpPost]
    public async Task<IActionResult> Create(Evento evento)
    {
        // Validación Nombre
        if (string.IsNullOrWhiteSpace(evento.Nombre))
            return BadRequest("El nombre es obligatorio.");
//Nombre: obligatorio, entre 3 y 100 caracteres.
        if (evento.Nombre.Length < 3 || evento.Nombre.Length > 100)
            return BadRequest("El nombre debe tener entre 3 y 100 caracteres.");

        // Validación Ciudad
        if (string.IsNullOrWhiteSpace(evento.Ciudad))
            return BadRequest("La ciudad es obligatoria.");
// Ciudad: obligatoria, entre 2 y 60 caracteres,
        if (evento.Ciudad.Length < 2 || evento.Ciudad.Length > 60)
            return BadRequest("La ciudad debe tener entre 2 a 60 espacios.");
//Normalizacion de ciudad: solo letras, espacios, guiones o apóstrofes.
        if (!Regex.IsMatch(
            evento.Ciudad,
            @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s'-]+$"))
            return BadRequest(
                "La ciudad solo puede contener letras, espacios, guiones o apóstrofes.");

        // Fecha: obligatoria, no puede ser una fecha ya pasada (debe ser hoy o en el futuro).
        if (evento.Fecha.Date < DateTime.Today)
            return BadRequest("La fecha no puede ser anterior a hoy.");

        // CapacidadTotal: obligatorio, entero mayor a 0.
        if (evento.CapacidadTotal <= 0)
            return BadRequest("La capacidad debe ser mayor que 0.");

        // PrecioBoleto: obligatorio, no puede ser negativo (0 es válido, para eventos gratuitos)
        if (evento.PrecioBoleto < 0)
            return BadRequest("El precio no puede ser negativo.");

        _db.Eventos.Add(evento);

        await _db.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = evento.Id },
            evento);
    }

    // PUT: api/Eventos/1
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Evento evento)
    {
        var eventoExistente = await _db.Eventos.FindAsync(id);

        if (eventoExistente == null)
            return NotFound("El evento no existe.");

        // Validación Nombre
        if (string.IsNullOrWhiteSpace(evento.Nombre))
            return BadRequest("El nombre es obligatorio.");
//Nombre: obligatorio, entre 3 y 100 caracteres.
        if (evento.Nombre.Length < 3 || evento.Nombre.Length > 100)
            return BadRequest("El nombre debe tener entre 3 y 100 caracteres.");

        // Validación Ciudad
        if (string.IsNullOrWhiteSpace(evento.Ciudad))
            return BadRequest("La ciudad es obligatoria.");
// Ciudad: obligatoria, entre 2 y 60 caracteres,
        if (evento.Ciudad.Length < 2 || evento.Ciudad.Length > 60)
            return BadRequest("La ciudad debe tener entre 2 a 60 espacios.");
//Normalizacion de ciudad: solo letras, espacios, guiones o apóstrofes.
        if (!Regex.IsMatch(
            evento.Ciudad,
            @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s'-]+$"))
            return BadRequest(
                "La ciudad solo puede contener letras, espacios, guiones o apóstrofes.");

        // Fecha: obligatoria, no puede ser una fecha ya pasada (debe ser hoy o en el futuro).
        if (evento.Fecha.Date < DateTime.Today)
            return BadRequest("La fecha no puede ser anterior a hoy.");

        // CapacidadTotal: obligatorio, entero mayor a 0.
        if (evento.CapacidadTotal <= 0)
            return BadRequest("La capacidad debe ser mayor que 0.");

        // PrecioBoleto: obligatorio, no puede ser negativo (0 es válido, para eventos gratuitos)
        if (evento.PrecioBoleto < 0)
            return BadRequest("El precio no puede ser negativo.");

        // Actualizar datos
        eventoExistente.Nombre = evento.Nombre;
        eventoExistente.Ciudad = evento.Ciudad;
        eventoExistente.Fecha = evento.Fecha;
        eventoExistente.CapacidadTotal = evento.CapacidadTotal;
        eventoExistente.PrecioBoleto = evento.PrecioBoleto;

        await _db.SaveChangesAsync();

        return Ok(eventoExistente);
    }

    // DELETE: api/Eventos/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var evento = await _db.Eventos.FindAsync(id);

        if (evento == null)
            return NotFound("El evento no existe.");

        _db.Eventos.Remove(evento);

        await _db.SaveChangesAsync();

        return NoContent();
    }
}