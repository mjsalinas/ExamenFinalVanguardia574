using System.ComponentModel.DataAnnotations;

namespace BibliotecaMonolito.Models
{
    public class Evento
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La ciudad es obligatoria.")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "La ciudad debe tener entre 2 y 60 caracteres.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s\-'\.]+$", ErrorMessage = "La ciudad solo puede contener letras, espacios, guiones o apóstrofes.")]
        public string Ciudad { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "La capacidad total es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La capacidad total debe ser un entero mayor a 0.")]
        public int CapacidadTotal { get; set; }

        [Required(ErrorMessage = "El precio del boleto es obligatorio.")]
        [Range(0, double.MaxValue, ErrorMessage = "El precio del boleto no puede ser negativo.")]
        public decimal PrecioBoleto { get; set; }

        // Relación 1 a N con Boletos
        public ICollection<Boleto> Boletos { get; set; } = new List<Boleto>();
    }
}