namespace ProyAdoPet.ViewModel
{
    public class SeguimientoListaVM
    {
        public int SolicitudId { get; set; }
        public string? Adoptante { get; set; }
        public string? DNI { get; set; }
        public string? Telefono { get; set; }
        public string? Mascota { get; set; }
        public string? CodigoContrato { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? UltimoControl { get; set; }

        //para indicar estado control - solo visual
        public int DiasDesdeUltimoControl => UltimoControl.HasValue
            ? (DateTime.Now - UltimoControl.Value).Days
            : (DateTime.Now - FechaInicio).Days;
    }
}
