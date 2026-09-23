using System.Globalization;
using System.Text.RegularExpressions;

namespace TicketExpressMonolito.Common;

public static class TextNormalizer
{
    public static string Normalizar(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto)) return string.Empty;
        texto = Regex.Replace(texto.Trim(), @"\s+", " ");
        var textInfo = CultureInfo.CurrentCulture.TextInfo;
        return textInfo.ToTitleCase(texto.ToLower());
    }
}