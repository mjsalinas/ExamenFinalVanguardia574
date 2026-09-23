using System.Globalization;
using System.Text.RegularExpressions;
using ExamenFinalVanguardia574.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExamenFinalVanguardia574.Controllers;


[ApiController]
[Route("api/[controller]")]
public class LibrosController : ControllerBase
{
    // Gutenberg ~1440: primer año razonable para un libro impreso.
    private const int AnioMinimoPublicacion = 1440;
    private const int TituloMinLength = 3;
    private const int TituloMaxLength = 200;

    private readonly LibraryDbContext _db;

