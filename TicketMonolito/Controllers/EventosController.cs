using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace TicketMonolito.Controllers;
public class EventosController : Controllers
{
    private readonly TicketContext _context;
    public EventosController(TicketContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _context.Eventos.Include(e => e.Boletos).ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var evento = await _context.Eventos.Include(e => e.Boletos).FirstOrDefaultAsync(e => e.Id == id);
        if (evento == null) return NotFound();
        return Ok(evento);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Evento evento)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        _context.Eventos.Add(evento);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = evento.Id }, evento);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Evento eventoDto)
    {
        var evento = await _context.Eventos.FindAsync(id);
        if (evento == null) return NotFound();

        evento.Nombre = TextNormalizer.Normalize(eventoDto.Nombre);
        evento.Ciudad = TextNormalizer.Normalize(eventoDto.Ciudad);
        evento.Fecha = eventoDto.Fecha;
        evento.CapacidadTotal = eventoDto.CapacidadTotal;
        evento.PrecioBoleto = eventoDto.PrecioBoleto;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var evento = await _context.Eventos.Include(e => e.Boletos).FirstOrDefaultAsync(e => e.Id == id);
        if (evento == null) return NotFound();

        // Regla de Negocio: No eliminar evento con boletos vendidos
        if (evento.Boletos.Any())
        {
            return BadRequest(new { mensaje = "No se puede eliminar un evento que ya tiene boletos vendidos." });
        }

        _context.Eventos.Remove(evento);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}