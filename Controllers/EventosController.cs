using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketExpress.Models;

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
        return Ok(eventos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var evento = await _db.Eventos.FindAsync(id);

        if (evento is null)
            return NotFound();

        return Ok(evento);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Evento evento)
    {
        var nuevoEvento = new Evento
        {
            Nombre = evento.Nombre,
            Ciudad = evento.Ciudad,
            Fecha = evento.Fecha,
            CapacidadTotal = evento.CapacidadTotal,
            PrecioBoleto = evento.PrecioBoleto
        };

        _db.Eventos.Add(nuevoEvento);
        await _db.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = nuevoEvento.Id },
            nuevoEvento);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Evento eventoActualizado)
    {
        var evento = await _db.Eventos.FindAsync(id);

        if (evento is null)
            return NotFound();

        evento.Nombre = eventoActualizado.Nombre;
        evento.Ciudad = eventoActualizado.Ciudad;
        evento.Fecha = eventoActualizado.Fecha;
        evento.CapacidadTotal = eventoActualizado.CapacidadTotal;
        evento.PrecioBoleto = eventoActualizado.PrecioBoleto;

        await _db.SaveChangesAsync();

        return Ok(evento);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var evento = await _db.Eventos.FindAsync(id);

        if (evento is null)
            return NotFound();

        var tieneBoletos = await _db.Boletos
            .AnyAsync(b => b.EventoId == id);

        if (tieneBoletos)
        {
            return Conflict(
                "No se puede eliminar un evento que tiene boletos vendidos.");
        }

        _db.Eventos.Remove(evento);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}