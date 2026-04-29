namespace ProyAdoPet.ViewModel
{
    public class SeguimientoDetalleVM
    {
        public int SolicitudId { get; set; }
        public string? MascotaNombre { get; set; }
        public string? FotoMascota { get; set; }
        public string? CodigoContrato { get; set; }
        public List<SeguimientoItemVM> Controles { get; set; } = new List<SeguimientoItemVM>();
    }
}
