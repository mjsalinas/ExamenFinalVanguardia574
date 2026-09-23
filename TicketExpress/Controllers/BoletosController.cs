using System.Diagnostics;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketExpress.Models;

namespace TicketExpress.Controllers;

[ApiController]
[Route("api/[controller]")]

public class BoletosController : ControllerBase
{

	private readonly LibraryDbContext _db;

	public LibrosController(LibraryDbContext db) => _db = db;

	[HttpPost]
	public async Task<IActionResult> Create(Boleto boleto)
	{
		// Transformación de datos (futuro AppService)
		AplicarTransformacion(boleto);

		if (boleto.NombreComprador.Length < 2 && boleto.NombreComprador.Length > 100)
			return BadRequest("el nombre debe ser entre 2 y 100 caracteres.");


		if (boleto.Cantidad <= 0)
			return BadRequest("la cantidad de boletos no puede ser 0 o menor");

		if (!EmailRegex.IsMatch(boleto.CorreoComprador))
			return BadRequest( "El correo electrónico no tiene un formato válido.");

		_db.boletos.Add(boleto);
		await _db.SaveChangesAsync();
		return CreatedAtAction(nameof(GetById), new { id = boleto.Id }, boleto);
	}

	private static void AplicarTransformacion(Boleto boleto)
	{
		boleto.NombreComprador = NormalizarNombre(boleto.NombreComprador);
		
	}
	private static string NormalizarNombre(string? nombrecomprador)
	{
		if (string.IsNullOrWhiteSpace(nombrecomprador))
			return string.Empty;

		var colapsado = Regex.Replace(nombrecomprador.Trim(), @"\s+", " ");
		var cultura = CultureInfo.GetCultureInfo("es-HN");
		return cultura.TextInfo.ToTitleCase(colapsado.ToLower(cultura));
	}
}