using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BibliotecaMonolito.Models
{
    public class Boleto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El EventoId es obligatorio.")]
        public int EventoId { get; set; }

        [JsonIgnore]
        public Evento? Evento { get; set; }

        [Required(ErrorMessage = "El nombre del comprador es obligatorio.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre del comprador debe tener entre 2 y 100 caracteres.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s\-'\.]+$", ErrorMessage = "El nombre del comprador solo puede contener letras, espacios, guiones o apóstrofes.")]
        public string NombreComprador { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo del comprador es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        public string CorreoComprador { get; set; } = string.Empty;

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser un entero mayor a 0.")]
        public int Cantidad { get; set; }

        public DateTime FechaCompra { get; set; }
    }
}