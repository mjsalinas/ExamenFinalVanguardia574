namespace ExamenFinalVanguardia574.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BoletosController : ControllerBase
{
    private readonly LibraryDbContext _db;

    public BoletosController(LibraryDbContext db) => _db = db;
    
[HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var boletos = await _db.Boletos.ToListAsync();
        return Ok(boletos);
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
}

