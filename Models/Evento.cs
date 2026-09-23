using System.ComponentModel.DataAnnotations;

namespace TicketExpress.Models;

public class Evento
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, MinimumLength = 3,
        ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    public string Ciudad { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public int CapacidadTotal { get; set; }
    public decimal PrecioBoleto { get; set; }
}