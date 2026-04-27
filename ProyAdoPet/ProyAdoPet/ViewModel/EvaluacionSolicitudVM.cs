namespace ProyAdoPet.ViewModel
{
    public class EvaluacionSolicitudVM
    {
        public int SolicitudId { get; set; }
        public string? NombrePostulante { get; set; }
        public string? DNI { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string? MotivoAdopcion { get; set; }
        public string? MascotaNombre { get; set; }
        public string? MascotaFoto { get; set; }
        public int EstadoActualId { get; set; }
    }
}
