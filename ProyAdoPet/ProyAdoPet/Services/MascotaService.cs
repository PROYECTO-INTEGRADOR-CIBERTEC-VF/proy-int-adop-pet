using ProyAdoPet.Models;
using ProyAdoPet.Repository;

namespace ProyAdoPet.Services
{
    public class MascotaService
    {
        IMascota _mascota;

        public MascotaService(IMascota mascota)
        {
            _mascota = mascota;
        }

        public IEnumerable<Mascota> MascotasDisponibles()
        {
            return _mascota.listado();
        }

        public IEnumerable<Estado> EstadosMascota()
        {
            return _mascota.ListarEstado();
        }
    }
}
