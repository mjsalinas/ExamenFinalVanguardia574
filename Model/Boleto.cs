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
public class Boleto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre del comprador es obligatorio.")]
    [StringLength(100, MinimumLength = 2,
        ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
    [RegularExpression(@"^[\p{L}\s'-]+$",
        ErrorMessage = "El nombre solo puede contener letras, espacios, guiones o apóstrofes.")]
    public string NombreComprador { get; set; }

    [Required(ErrorMessage = "El correo del comprador es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo debe tener un formato válido.")]
    public string CorreoComprador { get; set; }

    [Required(ErrorMessage = "La cantidad es obligatoria.")]
    [Range(1, int.MaxValue,
        ErrorMessage = "La cantidad debe ser un entero mayor a 0.")]
    public int Cantidad { get; set; }
}