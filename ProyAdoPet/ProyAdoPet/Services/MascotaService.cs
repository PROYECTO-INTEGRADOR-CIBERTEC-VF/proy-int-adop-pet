using ProyAdoPet.DAO;
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

        public async Task<bool> RegistrarMascota(Mascota objeto)
        {
            if (!string.IsNullOrEmpty(objeto.Nombre))
            {
                objeto.Nombre = objeto.Nombre.Trim();
            }
            return await Task.Run(() => _mascota.Registrar(objeto));
        }
    }
}
