using System.Text.RegularExpressions;
using TicketExpress.Models;

namespace TicketExpress.Validation;

public static class EventoValidator
{
    private const int NombreMinLength = 3;
    private const int NombreMaxLength = 100;
    private const int CiudadMinLength = 2;
    private const int CiudadMaxLength = 60;

    // Solo letras, espacios, guiones o apostrofes
    private static readonly Regex CiudadValidaRegex =
        new(@"^[\p{L}\s'\-]+$", RegexOptions.Compiled);

    // Devuelve el mensaje de error, o null si el evento es valido
    public static string? Validar(Evento evento)
    {
        if (string.IsNullOrWhiteSpace(evento.Nombre))
            return "El nombre del evento es obligatorio.";

        if (evento.Nombre.Length < NombreMinLength || evento.Nombre.Length > NombreMaxLength)
            return $"El nombre del evento debe tener entre {NombreMinLength} y {NombreMaxLength} caracteres.";

        if (string.IsNullOrWhiteSpace(evento.Ciudad))
            return "La ciudad del evento es obligatoria.";

        if (evento.Ciudad.Length < CiudadMinLength || evento.Ciudad.Length > CiudadMaxLength)
            return $"La ciudad debe tener entre {CiudadMinLength} y {CiudadMaxLength} caracteres.";

        if (!CiudadValidaRegex.IsMatch(evento.Ciudad))
            return "La ciudad solo puede contener letras, espacios, guiones o apostrofes.";

        if (evento.Fecha == default)
            return "La fecha del evento es obligatoria.";

        if (evento.Fecha.Date < DateTime.Today)
            return "La fecha del evento no puede ser una fecha pasada.";

        if (evento.CapacidadTotal <= 0)
            return "La capacidad total debe ser un numero entero mayor a cero.";

        if (evento.PrecioBoleto < 0)
            return "El precio del boleto no puede ser negativo.";

        return null;
    }
}