using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliotecaMonolito.Common;
using BibliotecaMonolito.Data;
using BibliotecaMonolito.Models;
using BibliotecaMonolito.Services;

namespace BibliotecaMonolito.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        private readonly TicketExpressContext _context;
        private readonly EventService _eventService;

        public EventosController(TicketExpressContext context, EventService eventService)
        {
            _context = context;
            _eventService = eventService;
        }

        // GET: api/Eventos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Evento>>> GetEventos()
        {
            return await _context.Eventos.Include(e => e.Boletos).ToListAsync();
        }

        // GET: api/Eventos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetEvento(int id)
        {
            var evento = await _context.Eventos.Include(e => e.Boletos).FirstOrDefaultAsync(e => e.Id == id);

            if (evento == null)
            {
                return NotFound(new { mensaje = "El evento especificado no existe." });
            }

            int disponibles = await _eventService.GetBoletosDisponiblesAsync(id);

            return Ok(new
            {
                evento.Id,
                evento.Nombre,
                evento.Ciudad,
                evento.Fecha,
                evento.CapacidadTotal,
                evento.PrecioBoleto,
                BoletosDisponibles = disponibles,
                evento.Boletos
            });
        }

        // POST: api/Eventos
        [HttpPost]
        public async Task<ActionResult<Evento>> PostEvento(Evento evento)
        {
            if (evento.Fecha.Date < DateTime.Today)
            {
                ModelState.AddModelError("Fecha", "La fecha del evento no puede ser una fecha pasada (debe ser hoy o en el futuro).");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            evento.Nombre = TextNormalizer.NormalizeName(evento.Nombre);
            evento.Ciudad = TextNormalizer.NormalizeName(evento.Ciudad);

            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvento), new { id = evento.Id }, evento);
        }

        // PUT: api/Eventos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvento(int id, Evento evento)
        {
            if (id != evento.Id)
            {
                return BadRequest(new { mensaje = "El ID del evento no coincide." });
            }

            if (evento.Fecha.Date < DateTime.Today)
            {
                ModelState.AddModelError("Fecha", "La fecha del evento no puede ser pasada.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            evento.Nombre = TextNormalizer.NormalizeName(evento.Nombre);
            evento.Ciudad = TextNormalizer.NormalizeName(evento.Ciudad);

            _context.Entry(evento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Eventos.AnyAsync(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Eventos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvento(int id)
        {
            var evento = await _context.Eventos.Include(e => e.Boletos).FirstOrDefaultAsync(e => e.Id == id);
            if (evento == null)
            {
                return NotFound(new { mensaje = "El evento no existe." });
            }

            // Regla de negocio: No se puede eliminar un evento que ya tiene al menos un boleto vendido
            if (evento.Boletos.Any())
            {
                return BadRequest(new { mensaje = "No se puede eliminar un Evento que ya tiene al menos un boleto vendido." });
            }

            _context.Eventos.Remove(evento);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}