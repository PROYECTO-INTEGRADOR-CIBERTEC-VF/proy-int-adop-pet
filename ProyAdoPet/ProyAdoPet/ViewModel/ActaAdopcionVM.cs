namespace ProyAdoPet.ViewModel
{
    public class ActaAdopcionVM
    {
        public int Folio { get; set; }
        public string? CodigoContrato { get; set; }
        public string? AdoptanteNombre { get; set; }
        public string? AdoptanteDNI { get; set; }
        public string? AdoptanteDireccion { get; set; }
        public string? MascotaNombre { get; set; }
        public DateTime FechaEmision { get; set; }
        public string? ObservacionesIniciales { get; set; }
    }
}
