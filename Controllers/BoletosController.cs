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
        var boletos = await _db.Boletos.ToListAsync();

        var compras = boletos
            .OrderByDescending(b => b.FechaCompra)
            .ToList();

        return Ok(compras);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0)
            return BadRequest("El id del boleto debe ser mayor a cero.");

        var boleto = await _db.Boletos.FirstOrDefaultAsync(b => b.Id == id);

        if (boleto is null)
            return NotFound($"No existe un boleto con el id {id}.");

        return Ok(boleto);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Boleto boleto)
    {
        boleto.FechaCompra = DateTime.UtcNow;

        _db.Boletos.Add(boleto);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = boleto.Id }, boleto);
    }
}