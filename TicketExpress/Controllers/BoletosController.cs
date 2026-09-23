using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketExpress.Data;
using TicketExpress.Models;

namespace TicketExpress.Controllers;

public class BoletosController : Controller
{
    private readonly ILogger<BoletosController> _logger;
    private readonly ApplicationDbContext _context;

    public BoletosController(ILogger<BoletosController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        var boletos = _context.Boletos.ToList();
        return View(boletos);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var boletos = _context.Boletos.ToList();
        return Json(boletos);
    }

    [HttpGet]
    public IActionResult GetById(int id)
    {
        var boleto = _context.Boletos.FirstOrDefault(b => b.Id == id);
        if (boleto == null)
        {
            return NotFound();
        }
        return Json(boleto);
    }

    [HttpPost]
    public IActionResult create([FromBody] Boletos boleto)
    {
        _context.Boletos.Add(boleto);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetById), new { id = boleto.Id }, boleto);
    }
}