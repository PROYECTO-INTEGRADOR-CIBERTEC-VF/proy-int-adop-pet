namespace ProyAdoPet.ViewModel
{
    public class SeguimientoListaVM
    {
        public int SolicitudId { get; set; }
        public string? Adoptante { get; set; }
        public string? Mascota { get; set; }
        public string? FotoMascota { get; set; }
        public string? CodigoContrato { get; set; }
        public DateTime? UltimoControl { get; set; }

        public int DiasDesdeUltimoControl => UltimoControl.HasValue
            ? (DateTime.Now - UltimoControl.Value).Days
            : 999;
    }
}
