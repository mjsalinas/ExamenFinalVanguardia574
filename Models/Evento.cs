namespace BibliotecaMonolito.Models;

public class Evento
{
    public int Id { get; set; }
    public string Nombre { get; set; } 
    public string Ciudad { get; set; } 
    public Datetime fecha { get; set; }
    public int CapacidadTotal { get; set; }
    public decimal PrecioBoleto { get; set; }
}
