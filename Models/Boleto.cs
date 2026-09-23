namespace BibliotecaMonolito.Models;

public class Boleto
{
    public int Id { get; set; }
    public int EventoId { get; set; }
    public string NombreComprador { get; set; } = string.Empty;
    public string CorreoComprador { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public Datetime FechaCompra { get; set; }
}
