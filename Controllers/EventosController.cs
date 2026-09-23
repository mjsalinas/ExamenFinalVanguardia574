using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
}