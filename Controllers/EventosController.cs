using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketExpress.Models;
using TicketExpress.Common;
using TicketExpress.Validation;

namespace TicketExpress.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventosController : ControllerBase
{
    private readonly TicketExpressDbContext _db;

    public EventosController(TicketExpressDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var eventos = await _db.Eventos.ToListAsync();

        var cartelera = eventos
            .OrderBy(e => e.Fecha)
            .ThenBy(e => e.Nombre, StringComparer.CurrentCultureIgnoreCase)
            .ToList();

        return Ok(cartelera);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0)
            return BadRequest("El id del evento debe ser mayor a cero.");

        var evento = await _db.Eventos.FirstOrDefaultAsync(e => e.Id == id);

        if (evento is null)
            return NotFound($"No existe un evento con el id {id}.");

        return Ok(evento);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Evento evento)
    {
        evento.Nombre = TextNormalizer.NormalizarNombrePropio(evento.Nombre);
        evento.Ciudad = TextNormalizer.NormalizarNombrePropio(evento.Ciudad);

        var error = EventoValidator.Validar(evento);
        if (error is not null)
            return BadRequest(error);
        _db.Eventos.Add(evento);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = evento.Id }, evento);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Evento eventoActualizado)
    {
        if (id <= 0)
            return BadRequest("El id del evento debe ser mayor a cero.");

        var evento = await _db.Eventos.FirstOrDefaultAsync(e => e.Id == id);

        if (evento is null)
            return NotFound($"No existe un evento con el id {id}.");

        evento.Nombre = TextNormalizer.NormalizarNombrePropio(eventoActualizado.Nombre);
        evento.Ciudad = TextNormalizer.NormalizarNombrePropio(eventoActualizado.Ciudad);

        evento.Fecha = eventoActualizado.Fecha;
        evento.CapacidadTotal = eventoActualizado.CapacidadTotal;
        evento.PrecioBoleto = eventoActualizado.PrecioBoleto;

        await _db.SaveChangesAsync();

        return Ok(evento);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
            return BadRequest("El id del evento debe ser mayor a cero.");

        var evento = await _db.Eventos.FirstOrDefaultAsync(e => e.Id == id);

        if (evento is null)
            return NotFound($"No existe un evento con el id {id}.");

        _db.Eventos.Remove(evento);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}