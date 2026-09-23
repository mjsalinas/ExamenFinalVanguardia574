using System.Globalization;
using System.Text.RegularExpressions;
using ExamenFinalVanguardia574.Data;
using ExamenFinalVanguardia574.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExamenFinalVanguardia574.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AutoresController : ControllerBase
{
    private const int NombreMinLength = 2;
    private const int NombreMaxLength = 100;
    private const int NacionalidadMinLength = 3;
    private const int NacionalidadMaxLength = 60;

    private static readonly Regex NombreValidoRegex =
        new(@"^[\p{L}\s'\-\.]+$", RegexOptions.Compiled);

    private readonly LibraryDbContext _db;

    public AutoresController(LibraryDbContext db) => _db = db;

    
    

   