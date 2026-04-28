using ProyAdoPet.Repository;
using ProyAdoPet.ViewModel;

namespace ProyAdoPet.Services
{
    public class AdopcionService
    {
        IAdopcion _adopcion;

        public AdopcionService (IAdopcion adopcion)
        {
            _adopcion = adopcion;
        }

        public IEnumerable<MisSolicitudesVM> ObtenerMisSolicitudes(int usuarioId)
        {
            return _adopcion.ObtenerMisSolicitudes(usuarioId);
        }

        public ActaAdopcionVM ObtenerDatosActa(int solicitudId)
        {
            return _adopcion.ObtenerDatosActa(solicitudId);
        }
    }
}
