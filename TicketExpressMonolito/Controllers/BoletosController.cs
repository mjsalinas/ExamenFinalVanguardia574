using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketExpressMonolito.Common;
using TicketExpressMonolito.Data;
using TicketExpressMonolito.Models;

namespace TicketExpressMonolito.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BoletosController : ControllerBase
{
    private readonly AppDbContext _context;
    public BoletosController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Boleto>>> GetAll()
        => await _context.Boletos.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Boleto>> GetById(int id)
    {
        var boleto = await _context.Boletos.FindAsync(id);
        if (boleto == null) return NotFound();
        return boleto;
    }

    [HttpPost]
    public async Task<ActionResult<Boleto>> Create(Boleto boleto)
    {
        // Validaciones de formato
        var errores = ValidarBoleto(boleto);
        if (errores.Any()) return BadRequest(new { errores });

        // Regla: evento inexistente
        var evento = await _context.Eventos.FindAsync(boleto.EventoId);
        if (evento == null) return NotFound(new { mensaje = "Evento no encontrado." });

        // Regla: no vender boletos de evento pasado
        if (evento.Fecha.Date < DateTime.Today)
            return Conflict(new { mensaje = "No se pueden vender boletos para un evento pasado." });

        // Regla: capacidad disponible
        var disponibles = await DisponibilidadHelper.BoletosDisponiblesAsync(_context, boleto.EventoId);
        if (boleto.Cantidad > disponibles)
            return Conflict(new { mensaje = $"Capacidad insuficiente. Disponibles: {disponibles}" });

        boleto.NombreComprador = TextNormalizer.Normalizar(boleto.NombreComprador);
        boleto.FechaCompra = DateTime.Now; // asignada en servidor

        _context.Boletos.Add(boleto);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = boleto.Id }, boleto);
    }

    private static List<string> ValidarBoleto(Boleto b)
    {
        var errores = new List<string>();

        if (string.IsNullOrWhiteSpace(b.NombreComprador) || b.NombreComprador.Length < 2 || b.NombreComprador.Length > 100
            || !System.Text.RegularExpressions.Regex.IsMatch(b.NombreComprador, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s\-']+$"))
            errores.Add("NombreComprador: obligatorio, entre 2 y 100 caracteres, solo letras, espacios, guiones o apóstrofes.");

        if (string.IsNullOrWhiteSpace(b.CorreoComprador)
            || !System.Text.RegularExpressions.Regex.IsMatch(b.CorreoComprador, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            errores.Add("CorreoComprador: obligatorio, formato de correo válido.");

        if (b.Cantidad <= 0)
            errores.Add("Cantidad: obligatorio, entero mayor a 0.");

        return errores;
    }
}