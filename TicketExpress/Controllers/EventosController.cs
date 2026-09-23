using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketExpress.Data;
using TicketExpress.Models;

namespace TicketExpress.Controllers;

public class EventosController : Controller
{
    private readonly ILogger<EventosController> _logger;
    private readonly ApplicationDbContext _context;

    public EventosController(ILogger<EventosController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        var eventos = _context.Eventos.ToList();
        return View(eventos);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var eventos = _context.Eventos.ToList();
        return Json(eventos);
    }

    [HttpGet]

    public IActionResult GetById(int id)
    {
        var evento = _context.Eventos.FirstOrDefault(e => e.Id == id);
        if (evento == null)
        {
            return NotFound();
        }
        return Json(evento);
    }

    [HttpPost]
    public IActionResult Create([FromBody] Evento evento)
    {
        _context.Eventos.Add(evento);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetById), new { id = evento.Id }, evento);
    }

    [HttpPut]
    public IActionResult Update(int id, [FromBody] Evento evento)
    {
        if (id != evento.Id)
        {
            return BadRequest();
        }

        _context.Eventos.Update(evento);
        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete]
    public IActionResult Delete(int id)
    {
        var evento = _context.Eventos.FirstOrDefault(e => e.Id == id);
        if (evento == null)
        {
            return NotFound();
        }

        _context.Eventos.Remove(evento);
        _context.SaveChanges();
        return NoContent();
    }


}

