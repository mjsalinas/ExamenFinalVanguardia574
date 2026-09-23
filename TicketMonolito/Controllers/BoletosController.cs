using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
public class BoletosController : Controllers;
{
    private readonly TicketContext _context;
    public BoletosController(TicketContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _context.Boletos.Include(b => b.Evento).ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var boleto = await _context.Boletos.Include(b => b.Evento).FirstOrDefaultAsync(b => b.Id == id);
        if (boleto == null) return NotFound();
        return Ok(boleto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Boleto boleto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // Regla de Negocio: Evento existente
        var evento = await _context.Eventos.Include(e => e.Boletos).FirstOrDefaultAsync(e => e.Id == boleto.EventoId);
        if (evento == null)
        {
            return NotFound(new { mensaje = "El evento especificado no existe." });
        }

        // Regla de Negocio: Evento no pasado
        if (evento.Fecha.Date < DateTime.Now.Date)
        {
            return BadRequest(new { mensaje = "No se pueden comprar boletos para un evento cuya fecha ya pasó." });
        }

        // Regla de Negocio: Capacidad disponible
        int disponibles = BusinessHelper.GetBoletosDisponibles(_context, boleto.EventoId);
        if (boleto.Cantidad > disponibles)
        {
            return BadRequest(new { mensaje = $"La cantidad solicitada supera los boletos disponibles ({disponibles} restantes)." });
        }

        boleto.FechaCompra = DateTime.Now;
        _context.Boletos.Add(boleto);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = boleto.Id }, boleto);
    }
}