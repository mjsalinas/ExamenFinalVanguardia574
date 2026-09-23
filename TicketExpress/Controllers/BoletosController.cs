using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace TicketExpress.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoletosController : ControllerBase
    {
        private readonly TicketExpressContext _context;

        public BoletosController(TicketExpressContext context)
        {
            _context = context;
        }

        // GET: 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Boleto>>> GetBoletos()
        {
            return await _context.Boletos.ToListAsync();
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Boleto>> GetBoleto(int id)
        {
            var boleto = await _context.Boletos.FindAsync(id);

            if (boleto == null)
            {
                return NotFound();
            }

            return boleto;
        }

        // POST: api/Boletos
        [HttpPost]
        public async Task<ActionResult<Boleto>> PostBoleto(Boleto boleto)
        {
            // Normalizacion
            boleto.NombreComprador = TextNormalizer.NormalizeName(boleto.NombreComprador);

            // Validaciones de Formato
            if (string.IsNullOrWhiteSpace(boleto.NombreComprador) || boleto.NombreComprador.Length < 2 || boleto.NombreComprador.Length > 100 || !Regex.IsMatch(boleto.NombreComprador, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s\-']+$"))
                return BadRequest("El nombre del comprador es obligatorio (2 a 100 caracteres) y solo puede contener letras, espacios, guiones o apóstrofes.");

            if (string.IsNullOrWhiteSpace(boleto.CorreoComprador) || !Regex.IsMatch(boleto.CorreoComprador, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return BadRequest("El correo del comprador es obligatorio y debe tener un formato válido.");

            if (boleto.Cantidad <= 0)
                return BadRequest("La cantidad de boletos a comprar debe ser mayor a 0.");

            // Reglas de Negocio
            var evento = await _context.Eventos.FindAsync(boleto.EventoId);
            if (evento == null)
            {
                return NotFound("El evento especificado no existe.");
            }

            // Regla de Negocio 2: No boletos para eventos pasados
            if (evento.Fecha.Date < DateTime.Today)
            {
                return BadRequest("No se puede crear un boleto para un evento cuya fecha ya pasó.");
            }

            // Regla de Negocio 1: Validar disponibilidad de boletos usando la función compartida
            int disponibles = await TicketHelper.CalcularBoletosDisponiblesAsync(_context, evento.Id);
            
            if (boleto.Cantidad > disponibles)
            {
                return BadRequest($"La cantidad solicitada ({boleto.Cantidad}) supera los boletos disponibles ({disponibles}) para este evento.");
            }

            _context.Boletos.Add(boleto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBoleto", new { id = boleto.Id }, boleto);
        }
    }
}