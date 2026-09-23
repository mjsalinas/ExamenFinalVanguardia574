
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketExpress.Models;
using System.Text.RegularExpressions;

namespace TicketExpress.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BoletosController : ControllerBase
{
    private readonly LibraryDbContext _db;

    public BoletosController(LibraryDbContext db) => _db = db;

    // GET: api/Boletos
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var boletos = await _db.Boletos.ToListAsync();

        return Ok(boletos);
    }

    // GET: api/Boletos/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var boleto = await _db.Boletos.FindAsync(id);

        if (boleto == null)
            return NotFound("El boleto no existe.");

        return Ok(boleto);
    }

    // POST: api/Boletos
    [HttpPost]
    public async Task<IActionResult> Create(Boleto boleto)
    {
        // Validación NombreComprador
        if (string.IsNullOrWhiteSpace(boleto.NombreComprador))
            return BadRequest("El nombre del comprador es obligatorio.");

        // NombreComprador: entre 2 y 100 caracteres
        if (boleto.NombreComprador.Length < 2 ||
            boleto.NombreComprador.Length > 100)
            return BadRequest(
                "El nombre del comprador debe tener entre 2 y 100 caracteres.");

        // NombreComprador: solo letras, espacios, guiones o apóstrofes
        if (!Regex.IsMatch(
            boleto.NombreComprador,
            @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s'-]+$"))
            return BadRequest(
                "El nombre del comprador solo puede contener letras, espacios, guiones o apóstrofes.");

        // Validación CorreoComprador
        if (string.IsNullOrWhiteSpace(boleto.CorreoComprador))
            return BadRequest("El correo del comprador es obligatorio.");

        // CorreoComprador: formato válido
        if (!Regex.IsMatch(
            boleto.CorreoComprador,
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            return BadRequest("El correo del comprador no tiene un formato válido.");

        // Validación Cantidad
        // Cantidad: entero mayor a 0
        if (boleto.Cantidad <= 0)
            return BadRequest("La cantidad debe ser mayor que 0.");

        // Validación EventoId
        var evento = await _db.Eventos.FindAsync(boleto.EventoId);

        if (evento == null)
            return BadRequest("El evento no existe.");

        // FechaCompra: se asigna en el servidor
        boleto.FechaCompra = DateTime.Now;

        _db.Boletos.Add(boleto);

        await _db.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = boleto.Id },
            boleto);
    }

    // PUT: api/Boletos/1
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Boleto boleto)
    {
        var boletoExistente = await _db.Boletos.FindAsync(id);

        if (boletoExistente == null)
            return NotFound("El boleto no existe.");

        // Validación NombreComprador
        if (string.IsNullOrWhiteSpace(boleto.NombreComprador))
            return BadRequest("El nombre del comprador es obligatorio.");

        if (boleto.NombreComprador.Length < 2 ||
            boleto.NombreComprador.Length > 100)
            return BadRequest(
                "El nombre del comprador debe tener entre 2 y 100 caracteres.");

        if (!Regex.IsMatch(
            boleto.NombreComprador,
            @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s'-]+$"))
            return BadRequest(
                "El nombre del comprador solo puede contener letras, espacios, guiones o apóstrofes.");

        // Validación CorreoComprador
        if (string.IsNullOrWhiteSpace(boleto.CorreoComprador))
            return BadRequest("El correo del comprador es obligatorio.");

        if (!Regex.IsMatch(
            boleto.CorreoComprador,
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            return BadRequest("El correo del comprador no tiene un formato válido.");

        // Validación Cantidad
        if (boleto.Cantidad <= 0)
            return BadRequest("La cantidad debe ser mayor que 0.");

        // Validación EventoId
        var evento = await _db.Eventos.FindAsync(boleto.EventoId);

        if (evento == null)
            return BadRequest("El evento no existe.");

        // Actualizar datos
        boletoExistente.EventoId = boleto.EventoId;
        boletoExistente.NombreComprador = boleto.NombreComprador;
        boletoExistente.CorreoComprador = boleto.CorreoComprador;
        boletoExistente.Cantidad = boleto.Cantidad;

        // No se modifica FechaCompra
        // La fecha original se mantiene.

        await _db.SaveChangesAsync();

        return Ok(boletoExistente);
    }

    // DELETE: api/Boletos/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var boleto = await _db.Boletos.FindAsync(id);

        if (boleto == null)
            return NotFound("El boleto no existe.");

        _db.Boletos.Remove(boleto);

        await _db.SaveChangesAsync();

        return NoContent();
    }
}