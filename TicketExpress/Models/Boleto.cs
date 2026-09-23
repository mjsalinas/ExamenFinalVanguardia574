namespace TicketExpress.Models

{
    public class Boleto
    {
        public int Id { get; set; }
        public int EventoId { get; set; }
        public string NombreComprador { get; set; } = string.Empty;
        public string CorreoComprador { get; set; } = string.Empty;
        public DateTime FechaCompra { get; set; }
        public int Cantidad { get; set; }
        public Evento? Evento { get; set; }
    }
}