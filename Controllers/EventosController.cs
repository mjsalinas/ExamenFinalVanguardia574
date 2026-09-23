using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExamenFinalVanguardia574.Models;

namespace ExamenFinalVanguardia574.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EventosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Eventos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Evento>>> GetEventos()
        {
            var eventos = await _context.Eventos.ToListAsync();

            return Ok(eventos);
        }

        // GET: api/Eventos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Evento>> GetEvento(int id)
        {
            var evento = await _context.Eventos
                .FirstOrDefaultAsync(e => e.Id == id);

            if (evento == null)
            {
                return NotFound(new
                {
                    mensaje = "El evento no existe."
                });
            }

            return Ok(evento);
        }

        // POST: api/Eventos
        [HttpPost]
        public async Task<ActionResult<Evento>> CrearEvento(Evento evento)
        {
            if (string.IsNullOrWhiteSpace(evento.Nombre))
            {
                return BadRequest(new
                {
                    mensaje = "El nombre del evento es obligatorio."
                });
            }

            if (string.IsNullOrWhiteSpace(evento.Ciudad))
            {
                return BadRequest(new
                {
                    mensaje = "La ciudad es obligatoria."
                });
            }

            if (evento.Fecha <= DateTime.Now)
            {
                return BadRequest(new
                {
                    mensaje = "La fecha del evento debe ser futura."
                });
            }

            if (evento.CapacidadTotal <= 0)
            {
                return BadRequest(new
                {
                    mensaje = "La capacidad debe ser mayor que 0."
                });
            }

            if (evento.PrecioBoleto < 0)
            {
                return BadRequest(new
                {
                    mensaje = "El precio del boleto no puede ser negativo."
                });
            }

            _context.Eventos.Add(evento);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetEvento),
                new { id = evento.Id },
                evento
            );
        }

        // PUT: api/Eventos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarEvento(
            int id,
            Evento evento)
        {
            if (id != evento.Id)
            {
                return BadRequest(new
                {
                    mensaje = "El ID de la URL no coincide con el ID del evento."
                });
            }

            var eventoExistente = await _context.Eventos
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventoExistente == null)
            {
                return NotFound(new
                {
                    mensaje = "El evento no existe."
                });
            }

            if (string.IsNullOrWhiteSpace(evento.Nombre))
            {
                return BadRequest(new
                {
                    mensaje = "El nombre del evento es obligatorio."
                });
            }

            if (string.IsNullOrWhiteSpace(evento.Ciudad))
            {
                return BadRequest(new
                {
                    mensaje = "La ciudad es obligatoria."
                });
            }

            if (evento.Fecha <= DateTime.Now)
            {
                return BadRequest(new
                {
                    mensaje = "La fecha del evento debe ser futura."
                });
            }

            if (evento.CapacidadTotal <= 0)
            {
                return BadRequest(new
                {
                    mensaje = "La capacidad debe ser mayor que 0."
                });
            }

            if (evento.PrecioBoleto < 0)
            {
                return BadRequest(new
                {
                    mensaje = "El precio del boleto no puede ser negativo."
                });
            }

            int boletosVendidos = await _context.Boletos
                .Where(b => b.EventoId == id)
                .SumAsync(b => (int?)b.Cantidad) ?? 0;

            if (evento.CapacidadTotal < boletosVendidos)
            {
                return BadRequest(new
                {
                    mensaje = "La capacidad no puede ser menor que los boletos ya vendidos.",
                    boletosVendidos = boletosVendidos
                });
            }

            eventoExistente.Nombre = evento.Nombre;
            eventoExistente.Ciudad = evento.Ciudad;
            eventoExistente.Fecha = evento.Fecha;
            eventoExistente.CapacidadTotal = evento.CapacidadTotal;
            eventoExistente.PrecioBoleto = evento.PrecioBoleto;

            await _context.SaveChangesAsync();

            return Ok(eventoExistente);
        }

        // DELETE: api/Eventos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarEvento(int id)
        {
            var evento = await _context.Eventos
                .FirstOrDefaultAsync(e => e.Id == id);

            if (evento == null)
            {
                return NotFound(new
                {
                    mensaje = "El evento no existe."
                });
            }

            _context.Eventos.Remove(evento);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Evento eliminado correctamente."
            });
        }
    }
}