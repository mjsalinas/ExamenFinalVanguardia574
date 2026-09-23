using System.Globalization;
using System.Text.RegularExpressions;

namespace TicketExpress.Common;

public static class TextNormalizer
{
    public static string NormalizarNombrePropio(string? texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
            return string.Empty;

        var limpio = Regex.Replace(texto.Trim(), @"\s+", " ");

        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(limpio.ToLower(CultureInfo.CurrentCulture));
    }
}