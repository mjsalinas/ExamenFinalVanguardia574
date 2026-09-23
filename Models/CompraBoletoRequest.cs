namespace TicketExpress.Models;

public class CompraBoletoRequest
{
    public int EventoId { get; set; }
    public string NombreComprador { get; set; } = string.Empty;
    public string CorreoComprador { get; set; } = string.Empty;
    public int Cantidad { get; set; }
}