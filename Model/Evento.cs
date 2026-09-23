namespace ExamenFinalVanguardia574.Model
{
    public class Evento
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Ciudad { get; set; }
        public DateTime Fecha { get; set; }
        public int CapacidadTotal { get; set; }
        public decimal PrecioBoleto { get; set; }
        public ICollection<Boleto> Boletos { get; set; }
    }
}




