using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketExpressMonolito.Common;
using TicketExpressMonolito.Data;
using TicketExpressMonolito.Models;

namespace TicketExpressMonolito.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventosController : ControllerBase
{
    private readonly AppDbContext _context;
    public EventosController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Evento>>> GetAll()
        => await _context.Eventos.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Evento>> GetById(int id)
    {
        var evento = await _context.Eventos.FindAsync(id);
        if (evento == null) return NotFound();
        return evento;
    }

    [HttpPost]
    public async Task<ActionResult<Evento>> Create(Evento evento)
    {
        var errores = ValidarEvento(evento);
        if (errores.Any()) return BadRequest(new { errores });

        evento.Nombre = TextNormalizer.Normalizar(evento.Nombre);
        evento.Ciudad = TextNormalizer.Normalizar(evento.Ciudad);

        _context.Eventos.Add(evento);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = evento.Id }, evento);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Evento evento)
    {
        if (id != evento.Id) return BadRequest();

        var errores = ValidarEvento(evento);
        if (errores.Any()) return BadRequest(new { errores });

        evento.Nombre = TextNormalizer.Normalizar(evento.Nombre);
        evento.Ciudad = TextNormalizer.Normalizar(evento.Ciudad);

        _context.Entry(evento).State = EntityState.Modified;
        try { await _context.SaveChangesAsync(); }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Eventos.AnyAsync(e => e.Id == id)) return NotFound();
            throw;
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var evento = await _context.Eventos.FindAsync(id);
        if (evento == null) return NotFound();

        // Regla de negocio: no eliminar Evento con boletos vendidos
        var tieneBoletos = await _context.Boletos.AnyAsync(b => b.EventoId == id);
        if (tieneBoletos) return Conflict(new { mensaje = "No se puede eliminar un evento con boletos vendidos." });

        _context.Eventos.Remove(evento);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // ===== Validaciones de formato Evento =====
    private static List<string> ValidarEvento(Evento e)
    {
        var errores = new List<string>();

        if (string.IsNullOrWhiteSpace(e.Nombre) || e.Nombre.Length < 3 || e.Nombre.Length > 100)
            errores.Add("Nombre: obligatorio, entre 3 y 100 caracteres.");

        if (string.IsNullOrWhiteSpace(e.Ciudad) || e.Ciudad.Length < 2 || e.Ciudad.Length > 60
            || !System.Text.RegularExpressions.Regex.IsMatch(e.Ciudad, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s\-']+$"))
            errores.Add("Ciudad: obligatoria, entre 2 y 60 caracteres, solo letras, espacios, guiones o apóstrofes.");

        if (e.Fecha == default || e.Fecha.Date < DateTime.Today)
            errores.Add("Fecha: obligatoria, debe ser hoy o en el futuro.");

        if (e.CapacidadTotal <= 0)
            errores.Add("CapacidadTotal: obligatorio, entero mayor a 0.");

        if (e.PrecioBoleto < 0)
            errores.Add("PrecioBoleto: no puede ser negativo.");

        return errores;
    }
}