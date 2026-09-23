using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketExpress.Models;

using System;

namespace TicketExpress.Controllers
{
   public class BoletosController : ControllerBase
    {
private readonly LibraryDbContext _db;

    public BoletosController(LibraryDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var Boletos = await _db.Boletos.ToListAsync();
        return Ok(Boletos);
    }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var boleto = await _db.Boletos.FindAsync(id);
        if (boleto == null)
        {
            return NotFound();
        }
        return Ok(boleto);

    }
    [HttpGet("all")]
    public async Task<IActionResult> GetAllBoletos()
    {
        var Boletos = await _db.Boletos.ToListAsync();
        return Ok(Boletos);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Boleto boleto)
    {
        _db.Boletos.Add(boleto);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = boleto.Id }, boleto);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var boleto = await _db.Boletos.FindAsync(id);
        if (boleto == null)
        {
            return NotFound();
        }
        _db.Boletos.Remove(boleto);
        await _db.SaveChangesAsync();
        return NoContent();
    }

}