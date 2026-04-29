namespace ProyAdoPet.ViewModel
{
    public class DetalleSeguimientoVM
    {
        //Datos de adopcion
        public int SolicitudId { get; set; }
        public string? CodigoContrato { get; set; }
        public string? MascotaNombre { get; set; }
        public string? AdoptanteNombre { get; set; }

        //Historial seguimiento
        public List<SeguimientoItemVM> Controles { get; set; }
    }

    //clase historial seguimiento
    public class SeguimientoItemVM
    {
        public int Id { get; set; }
        public DateTime FechaControl { get; set; }
        public string? TipoControl { get; set; }
        public string? EstadoSalud { get; set; }
        public string? Observaciones { get; set; }
        public DateTime? ProximoControl { get; set; }
    }
}
