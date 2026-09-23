using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketExpress.Models;

using System;

namespace TicketExpress.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
private readonly LibraryDbContext _db;

    public EventosController(LibraryDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var Eventos = await _db.Eventos.ToListAsync();
        return Ok(Eventos);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Id must be a positive integer.");
        }

        var evento = await _db.Eventos.FindAsync(id);
        if (evento == null)
        {
            return NotFound();
        }
        return Ok(evento);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Evento evento)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        _db.Eventos.Add(evento);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = evento.Id }, evento);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
        {
            return BadRequest("Id must be a positive integer.");
        }

        var evento = await _db.Eventos.FindAsync(id);
        if (evento == null)
        {
            return NotFound();
        }
        _db.Eventos.Remove(evento);
        await _db.SaveChangesAsync();
        return NoContent();
    }
    [HttpGet("all")]
    public async Task<IActionResult> GetAllEventos()
    {
        var Eventos = await _db.Eventos.ToListAsync();
        return Ok(Eventos);
    }

}
}
