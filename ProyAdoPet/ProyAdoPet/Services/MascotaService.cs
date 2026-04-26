using ProyAdoPet.Constants;
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
    
    //  HU07 → OBTENER
        public Mascota ObtenerMascota(int id)
        {
            return _mascota.Obtener(id);
        }

        // 🔥 HU07 → ACTUALIZAR
        public async Task<bool> ActualizarMascota(Mascota objeto)
        {
            if (!string.IsNullOrEmpty(objeto.Nombre))
            {
                objeto.Nombre = objeto.Nombre.Trim();
            }

            return await Task.Run(() => _mascota.Actualizar(objeto));
        }

        //HU-09: ELIMINAR MASCOTA
        public bool EliminarMascota(int id)
        {
            if (id <= 0)
            {
                return false;
            }

            var mascotaActual = _mascota.Obtener(id);

            if (mascotaActual == null)
            {
                return false;
            }

            if (mascotaActual.Estado != EstadosConstantes.Disponible)
            {
                return false;
            }

            return _mascota.Eliminar(id);
        }

    }
}

