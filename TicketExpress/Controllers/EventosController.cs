using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketExpress.Models;

using System;

namespace TicketExpress.Controllers
{
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
    }
}