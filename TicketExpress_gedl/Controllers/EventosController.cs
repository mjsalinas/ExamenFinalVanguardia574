using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;
using TicketExpress_gedl.Models;

namespace TicketExpress_gedl.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventosController : ControllerBase
{
    private readonly LibraryDbContext _db;

    public EventosController(LibraryDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var eventos = await _db.Boletos.ToListAsync();

        // Transformación de lectura (futuro AppService): catálogo ordenado por nombre.
        var catalogoDeEventos = eventos.ToList();

        return Ok(catalogoDeEventos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0)
            return BadRequest("El identificador del evento debe ser mayor que cero.");

        var evento = await _db.Eventos.FirstOrDefaultAsync(e => e.Id == id);
        if (evento is null) return NotFound();
        return Ok(evento);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Evento evento)
    {

            //nombre: obligatorio, entre 3 y 100 caracteres
            if(evento.Nombre.Length < 3 || evento.Nombre.Length > 100)
            return BadRequest("El nombre del evento debe tener entre 3 y 100 caracteres.");

            //Ciudad: obligatoria, entre 2 y 60 caracteres, solo letras, espacios, guiones o apóstrofes.
            if(evento.Ciudad.Length < 2 || evento.Ciudad.Length > 60)
                return BadRequest("La ciudad del evento debe tener entre 2 y 60 caracteres.");

        //Fecha: obligatoria, no puede ser una fecha ya pasada (debe ser hoy o en el futuro).
        if(evento.Fecha < DateTime.Today)
            return BadRequest("La fecha del evento no puede ser una fecha pasada.");


        //• CapacidadTotal: obligatorio, entero mayor a 0
        if(evento.CapacidadTotal <= 0)
            return BadRequest("La capacidad total del evento es obligatoria y debe ser un número mayor que cero.");

        //• PrecioBoleto: obligatorio, no puede ser negativo (0 es válido, para eventos gratuitos)
        if(evento.PrecioBoleto < 0)
            return BadRequest("El precio del boleto es obligatorio y no puede ser negativo.");  


        _db.Eventos.Add(evento);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), new { id = evento.Id }, evento);
    }


}
