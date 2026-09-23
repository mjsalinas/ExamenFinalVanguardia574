namespace TicketExpress.Models
{
    public class Boleto
    {
        public int Id { get; set; }
        public Evento? EventoId { get; set; }
        public string NombreComprador { get; set; } = string.Empty;
        public string CorreoComprador { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public DateTime? FechaCompra { get; set; }
    }
}