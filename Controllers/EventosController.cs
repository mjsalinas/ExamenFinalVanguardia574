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
}