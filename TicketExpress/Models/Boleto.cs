namespace TicketExpress.Models
{
    public class Boleto
    {
        public int Id { get; set; }
        public int EventoId { get; set; }
        public string NombreComprador { get; set; } = string.Empty;
        public string CorreoComprador { get; set; } = string.Empty;
        public string FechaCompra { get; set; } = string.Empty;
          
    }}