using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExamenFinalVanguardia574.Models;

namespace ExamenFinalVanguardia574.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoletosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BoletosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Boletos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Boleto>>> GetBoletos()
        {
            var boletos = await _context.Boletos.ToListAsync();

            return Ok(boletos);
        }

        // GET: api/Boletos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Boleto>> GetBoleto(int id)
        {
            var boleto = await _context.Boletos
                .FirstOrDefaultAsync(b => b.Id == id);

            if (boleto == null)
            {
                return NotFound(new
                {
                    mensaje = "El boleto no existe."
                });
            }

            return Ok(boleto);
        }

        // GET: api/Boletos/evento/5
        [HttpGet("evento/{eventoId}")]
        public async Task<ActionResult<IEnumerable<Boleto>>> GetBoletosPorEvento(
            int eventoId)
        {
            // Verificar que el evento exista
            var eventoExiste = await _context.Eventos
                .AnyAsync(e => e.Id == eventoId);

            if (!eventoExiste)
            {
                return NotFound(new
                {
                    mensaje = "El evento no existe."
                });
            }

            var boletos = await _context.Boletos
                .Where(b => b.EventoId == eventoId)
                .ToListAsync();

            return Ok(boletos);
        }

        // POST: api/Boletos
        [HttpPost]
        public async Task<ActionResult<Boleto>> ComprarBoleto(
            Boleto boleto)
        {
            // Buscar el evento
            var evento = await _context.Eventos
                .FirstOrDefaultAsync(e => e.Id == boleto.EventoId);

            if (evento == null)
            {
                return NotFound(new
                {
                    mensaje = "El evento no existe."
                });
            }

            // Validar cantidad
            if (boleto.Cantidad <= 0)
            {
                return BadRequest(new
                {
                    mensaje = "La cantidad de boletos debe ser mayor que 0."
                });
            }

            // Validar nombre
            if (string.IsNullOrWhiteSpace(boleto.NombreComprador))
            {
                return BadRequest(new
                {
                    mensaje = "El nombre del comprador es obligatorio."
                });
            }

            // Validar correo
            if (string.IsNullOrWhiteSpace(boleto.CorreoComprador))
            {
                return BadRequest(new
                {
                    mensaje = "El correo del comprador es obligatorio."
                });
            }

            // Calcular boletos vendidos
            int boletosVendidos = await _context.Boletos
                .Where(b => b.EventoId == boleto.EventoId)
                .SumAsync(b => (int?)b.Cantidad) ?? 0;

            // Calcular boletos disponibles
            int boletosDisponibles =
                evento.CapacidadTotal - boletosVendidos;

            // Verificar disponibilidad
            if (boleto.Cantidad > boletosDisponibles)
            {
                return BadRequest(new
                {
                    mensaje = "No hay suficientes boletos disponibles.",
                    capacidadTotal = evento.CapacidadTotal,
                    boletosVendidos = boletosVendidos,
                    boletosDisponibles = boletosDisponibles,
                    boletosSolicitados = boleto.Cantidad
                });
            }

            // Registrar automáticamente la fecha de compra
            boleto.FechaCompra = DateTime.Now;

            _context.Boletos.Add(boleto);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetBoleto),
                new { id = boleto.Id },
                boleto
            );
        }

        // DELETE: api/Boletos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelarBoleto(int id)
        {
            var boleto = await _context.Boletos
                .FirstOrDefaultAsync(b => b.Id == id);

            if (boleto == null)
            {
                return NotFound(new
                {
                    mensaje = "El boleto no existe."
                });
            }

            _context.Boletos.Remove(boleto);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensaje = "Boleto cancelado correctamente."
            });
        }
    }
}