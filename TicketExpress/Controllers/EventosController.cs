using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketExpress.Models;

namespace TicketExpress.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventosController : ControllerBase
{
    private readonly LibraryDbContext _db;

    public EventosController(LibraryDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var Evento = await _db.Evento.ToListAsync();
        return Ok(Evento);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Evento eventoActualizado)
    {
        var evento = await _db.Evento.FindAsync(id);
        if (evento is null) return NotFound();

        evento.Nombre = eventoActualizado.Nombre;
        evento.Ciudad = eventoActualizado.Ciudad;
        evento.Fecha = eventoActualizado.Fecha;
        evento.CapacidadTotal = eventoActualizado.CapacidadTotal;
        evento.PrecioBoleto = eventoActualizado.PrecioBoleto;

        await _db.SaveChangesAsync();
        return Ok(evento);
    }
}
