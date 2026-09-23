namespace TicketExpress_gedl.Models;

public class Evento
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Ciudad { get; set; } = string.Empty;
    public DateTime Fecha { get; set; } = DateTime.MinValue;
    public int CapacidadTotal { get; set; } = 0;
}
