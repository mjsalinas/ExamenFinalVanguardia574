using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketExpress.Models;


namespace TicketExpress.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly LibraryDbContext _db;

        public EventosController(LibraryDbContext db) => _db = db;


        private const int NombreMaxLength = 100;
        private const int CiudadNombreMinLength = 3;
        private const int CiudadNombreMaxLength = 100;

        [HttpPost]
        public async Task<IActionResult> Create(Evento evento)
        {
            // Transformación de datos (futuro AppService)
            AplicarTransformacion(evento);

           private const int NombreMinLength = 3;
   

        private static void AplicarTransformacion(Evento evento)
    {
        evento.Ciudad = NormalizarNombre(evento.Ciudad);

    }
    private static string NormalizarNombre(string? nombreciudad)
    {
        if (string.IsNullOrWhiteSpace(nombreciudad))
            return string.Empty;

        var colapsado = Regex.Replace(nombreciudad.Trim(), @"\s+", " ");
        var cultura = CultureInfo.GetCultureInfo("es-HN");
        return cultura.TextInfo.ToTitleCase(colapsado.ToLower(cultura));
    
}



}



    

