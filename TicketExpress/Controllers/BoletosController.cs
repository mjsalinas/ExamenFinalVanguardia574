using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketExpress.Models;

namespace TicketExpress.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BoletosController : ControllerBase
{
    private readonly LibraryDbContext _db;

    public BoletosController(LibraryDbContext db) => _db = db;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var Boleto = await _db.Boleto.ToListAsync();
        return Ok(Boleto);
    }
}
