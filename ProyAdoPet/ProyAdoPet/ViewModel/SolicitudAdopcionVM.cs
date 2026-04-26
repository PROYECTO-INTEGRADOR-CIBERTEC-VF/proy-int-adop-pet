namespace ProyAdoPet.ViewModel
{
    public class SolicitudAdopcionVM
    {
        public int Id { get; set; }

        public string? NombrePostulante { get; set; }
        public string? DNI { get; set; }

        public string? NombreMascota { get; set; }
        public string? FotoMascota { get; set; }

        public DateTime FechaCreacion { get; set; }
        public string? EstadoNombre { get; set; }
        public int EstadoId { get; set; }
    }
}
