using ProyAdoPet.Models;
using ProyAdoPet.Repository;
using System.Collections.Generic;

namespace ProyAdoPet.Services
{
    public class MascotaService
    {
        private readonly IMascota _mascota;

        public MascotaService(IMascota mascota)
        {
            _mascota = mascota;
        }

        public IEnumerable<Mascota> MascotasDisponibles()
        {
            return _mascota.Listado();
        }

        public Mascota ObtenerMascotaPorId(int id)
        {
            return _mascota.ObtenerMascotaPorId(id);
        }

        public IEnumerable<Mascota> FiltrarMascotas(int? edad, string tipo, string tamano)
        {
            return _mascota.FiltrarMascotas(edad, tipo, tamano);
        }
    }
}
