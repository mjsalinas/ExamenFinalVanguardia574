namespace TicketExpress.Models
{
    public class Evento
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
         public string Ciudad { get; set; } = string.Empty;
         public string Fecha { get; set; } = string.Empty;
         public int CapacidadTotal { get; set; }  
         public float PrecioBoleto { get; set; }  
    }
}