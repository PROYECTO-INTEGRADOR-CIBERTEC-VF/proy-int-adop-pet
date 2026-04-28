namespace ProyAdoPet.Models
{
    public class CitaAdopcion
    {
        public int Id { get; set; }
        public int SolicitudId { get; set; }
        public DateTime FechaCita { get; set; }
        public string? Lugar { get; set; }
        public string? Notas { get; set; }
    }
}
