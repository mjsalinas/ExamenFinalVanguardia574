using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketExpress.Models;

namespace TicketExpress.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BoletosController : ControllerBase
{
    private readonly TicketExpressDbContext _db;

    public BoletosController(TicketExpressDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var boletos = await _db.Boletos
            .Include(b => b.Evento)
            .ToListAsync();

        return Ok(boletos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var boleto = await _db.Boletos
            .Include(b => b.Evento)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (boleto is null)
            return NotFound();

        return Ok(boleto);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CompraBoletoRequest solicitud)
    {
        var evento = await _db.Eventos.FindAsync(solicitud.EventoId);

        if (evento is null)
            return NotFound("El evento especificado no existe.");

        var boleto = new Boleto
        {
            EventoId = solicitud.EventoId,
            NombreComprador = solicitud.NombreComprador,
            CorreoComprador = solicitud.CorreoComprador,
            Cantidad = solicitud.Cantidad,
            FechaCompra = DateTime.Now
        };

        _db.Boletos.Add(boleto);
        await _db.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = boleto.Id },
            boleto);
    }
}