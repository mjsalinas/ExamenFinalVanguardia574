namespace ExamenFinalVanguardia574.Model
{
    public class Boleto
    {
        public int Id { get; set; }
        public string NombreComprador { get; set; }
        public string CorreoComprador { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaCompra { get; set; }
        public int EventoId { get; set; }
        public Evento Evento { get; set; }
    }
}
