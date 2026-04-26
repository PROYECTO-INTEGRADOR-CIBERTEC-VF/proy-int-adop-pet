using Microsoft.Extensions.Hosting;
using ProyAdoPet.Constants;
using ProyAdoPet.DAO;
using ProyAdoPet.Models;
using ProyAdoPet.Repository;

namespace ProyAdoPet.Services
{
    public class MascotaService
    {
        IMascota _mascota;
        private readonly IWebHostEnvironment _hostEnvironment;

        public MascotaService(IMascota mascota, IWebHostEnvironment hostEnvironment)
        {
            _mascota = mascota;
            _hostEnvironment = hostEnvironment;
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
            var mascota = _mascota.Obtener(id);

            if (mascota == null || mascota.Estado != EstadosConstantes.Disponible)
            {
                return false;
            }

            bool seEliminoDB = _mascota.Eliminar(id);

            if (seEliminoDB)
            {
                if (!string.IsNullOrEmpty(mascota.FotoMascota))
                {
                    string rutaFoto = Path.Combine(_hostEnvironment.WebRootPath, "fotos", mascota.FotoMascota);

                    if (System.IO.File.Exists(rutaFoto))
                    {
                        System.IO.File.Delete(rutaFoto);
                    }
                }
                return true;
            }
            return false;
        }

    }
}

