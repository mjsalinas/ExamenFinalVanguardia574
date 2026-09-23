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
    public class BoletosController : ControllerBase
    {
        private readonly TicketExpressContext _context;
        private readonly EventService _eventService;

        public BoletosController(TicketExpressContext context, EventService eventService)
        {
            _context = context;
            _eventService = eventService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Boleto>>> GetBoletos()
        {
            return await _context.Boletos.Include(b => b.Evento).ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Boleto>> GetBoleto(int id)
        {
            var boleto = await _context.Boletos.Include(b => b.Evento).FirstOrDefaultAsync(b => b.Id == id);

            if (boleto == null)
            {
                return NotFound(new { mensaje = "El boleto especificado no existe." });
            }

            return boleto;
        }


        [HttpPost]
        public async Task<ActionResult<Boleto>> PostBoleto(Boleto boleto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var evento = await _context.Eventos
                .Include(e => e.Boletos)
                .FirstOrDefaultAsync(e => e.Id == boleto.EventoId);

            if (evento == null)
            {
                return BadRequest(new { mensaje = "No se puede crear un Boleto para un Evento inexistente." });
            }


            if (evento.Fecha.Date < DateTime.Today)
            {
                return BadRequest(new { mensaje = "No se puede comprar boletos para un Evento cuya fecha ya pasó." });
            }


            int disponibles = await _eventService.GetBoletosDisponiblesAsync(evento.Id);
            if (boleto.Cantidad > disponibles)
            {
                return BadRequest(new { mensaje = $"La cantidad solicitada ({boleto.Cantidad}) supera los boletos disponibles ({disponibles}) para este evento." });
            }

            // 
            boleto.NombreComprador = TextNormalizer.NormalizeName(boleto.NombreComprador);


            boleto.FechaCompra = DateTime.Now;

            _context.Boletos.Add(boleto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBoleto), new { id = boleto.Id }, boleto);
        }
    }
}