namespace TicketExpress.Modelsme

{
    public class Evento
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Ciudad { get; set; }
        public DateTime Fecha { get; set; }
        public string CapacidadTotal { get; set; }

        public string PrecioBoleto { get; set; }
    }
}