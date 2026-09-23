namespace ExamenFinalVanguardia574.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventosController : ControllerBase

{
    private readonly LibraryDbContext _db;

    public EventosController(LibraryDbContext db) => _db = db;

 [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var eventos = await _db.Eventos.ToListAsync();
        return Ok(eventos);
    }

[HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var evento = await _db.Eventos.FindAsync(id);
        if (evento == null)
        {
            return NotFound();
        }
        return Ok(evento);
    }
}