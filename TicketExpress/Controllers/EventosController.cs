using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace TicketExpress.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
        // Se estandarizó el nombre a _context (antes tenías _db y _context mezclados)
        private readonly TicketExpressContext _context;

        public EventosController(TicketExpressContext context)
        {
            _context = context;
        }

        // GET: api/Eventos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Evento>>> GetEventos()
        {
            return await _context.Eventos.ToListAsync();
        }

        // GET: api/Eventos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Evento>> GetEvento(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);

            if (evento == null)
            {
                return NotFound();
            }

            return evento;
        }

        // POST: api/Eventos
        [HttpPost]
        public async Task<ActionResult<Evento>> PostEvento(Evento evento)
        {
            //Normalizacion
            evento.Nombre = TextNormalizer.NormalizeName(evento.Nombre);
            evento.Ciudad = TextNormalizer.NormalizeName(evento.Ciudad);

            // Validaciones de Formato
            if (string.IsNullOrWhiteSpace(evento.Nombre) || evento.Nombre.Length < 3 || evento.Nombre.Length > 100)
                return BadRequest("El nombre es obligatorio y debe tener entre 3 y 100 caracteres.");

            
            if (string.IsNullOrWhiteSpace(evento.Ciudad) || evento.Ciudad.Length < 2 || evento.Ciudad.Length > 60 || !Regex.IsMatch(evento.Ciudad, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s\-']+$"))
                return BadRequest("La ciudad es obligatoria (2 a 60 caracteres) y solo puede contener letras, espacios, guiones o apóstrofes.");

            if (evento.Fecha.Date < DateTime.Today)
                return BadRequest("La fecha del evento no puede ser en el pasado.");

            if (evento.CapacidadTotal <= 0)
                return BadRequest("La capacidad total debe ser mayor a 0.");

            if (evento.PrecioBoleto < 0)
                return BadRequest("El precio del boleto no puede ser negativo.");

            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvento", new { id = evento.Id }, evento);
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvento(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null)
            {
                return NotFound();
            }

            // Regla: No eliminar si ya tiene boletos vendidos
            bool tieneBoletos = await _context.Boletos.AnyAsync(b => b.EventoId == id);
            if (tieneBoletos)
            {
                return BadRequest("No se puede eliminar un evento que ya tiene boletos vendidos.");
            }

            _context.Eventos.Remove(evento);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}