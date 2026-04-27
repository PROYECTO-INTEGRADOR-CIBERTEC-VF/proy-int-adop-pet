using ProyAdoPet.Models;

namespace ProyAdoPet.ViewModel
{
    public class FiltroMascotaVM
    {
        public string? Nombre { get; set; }
        public string? Edad { get; set; }
        public int? EstadoId { get; set; }

        public IEnumerable<Mascota> Resultados { get; set; } = new List<Mascota>();
        public IEnumerable<Estado> Estados { get; set; } = new List<Estado>();
        public bool BusquedaRealizada { get; set; } = false;
    }
}
