namespace ProyAdoPet.ViewModel
{
    public class MisSolicitudesVM
    {
        public int SolicitudId { get; set; }
        public string? MascotaNombre { get; set; }
        public string? MascotaFoto { get; set; }
        public int? EstadoId { get; set; }
        public string? EstadoNombre { get; set; }
        public DateTime FechaEnvio { get; set; }
        public DateTime? FechaCita { get; set; }
        public string? LugarCita { get; set; }
        public string? NotasCita { get; set; }

        public int PorcentajeProgreso => EstadoId switch
        {
            1 => 25,  // Pendiente
            2 => 60,  // Citado
            3 => 100, // Aprobado
            4 => 100, // Rechazado
            _ => 0
        };
    }
}
