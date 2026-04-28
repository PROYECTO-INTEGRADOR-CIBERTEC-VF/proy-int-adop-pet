using System.ComponentModel.DataAnnotations;

namespace ProyAdoPet.Models
{
    public class SolicitudAdopcion
    {
        public int Id { get; set; }

        public int MascotaId { get; set; }

        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "Tu nombre es obligatorio")]
        public string? NombreCompleto { get; set; }

        [Required(ErrorMessage = "El DNI es necesario para el contrato")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "El DNI debe tener 8 dígitos")]
        public string? DNI { get; set; }

        [Required(ErrorMessage = "Danos un teléfono para coordinar la cita")]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria")]
        public string? Direccion { get; set; }

        [Required(ErrorMessage = "Cuéntanos por qué quieres adoptar")]
        public string? MotivoAdopcion { get; set; }

        public int EstadoSolicitudId { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
