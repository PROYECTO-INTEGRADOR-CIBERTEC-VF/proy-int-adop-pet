namespace ProyAdoPet.ViewModel
{
    public class DetalleSeguimientoVM
    {
        public int SolicitudId { get; set; }
        public string? CodigoContrato { get; set; }
        public string? MascotaNombre { get; set; }
        public string? AdoptanteNombre { get; set; }
        public List<SeguimientoItemVM>? Controles { get; set; }
    }

    public class SeguimientoItemVM
    {
        public int Id { get; set; }
        public DateTime FechaProgramada { get; set; }
        public string? TipoControl { get; set; }
        public string? Responsable { get; set; }
        public string? ObservacionInicial { get; set; }
        public string EstadoVisita { get; set; } = "Pendiente"; 

        //datos visita
        public DateTime? FechaRealizada { get; set; }
        public string? Resultado { get; set; }
        public string? Comentarios { get; set; }
        public string? FotoEvidencia { get; set; }
    }
}