using System.ComponentModel.DataAnnotations;

namespace TicketExpress.Models

{
    public class Evento
    {
        [Range(1, int.MaxValue, ErrorMessage = "Id es obligatorio y debe ser un entero positivo.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombre es obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Nombre debe tener entre 3 y 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ciudad es obligatoria.")]
        [RegularExpression(@"^[\p{L}\s\-']{2,60}$", ErrorMessage = "Ciudad debe tener entre 2 y 60 caracteres y solo puede incluir letras, espacios, guiones o apóstrofes.")]
        public string Ciudad { get; set; } = string.Empty;

        [Range(typeof(DateTime), "1900-01-01", "9999-12-31", ErrorMessage = "Fecha es obligatoria y debe ser una fecha válida.")]
        public DateTime Fecha { get; set; }
        public string CapacidadTotal { get; set; } = string.Empty;

        public string PrecioBoleto { get; set; } = string.Empty;
    }
}