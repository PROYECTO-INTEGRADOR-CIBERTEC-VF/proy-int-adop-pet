using ProyAdoPet.ViewModel;

namespace ProyAdoPet.Repository
{
    public interface IAdopcion
    {
        IEnumerable<MisSolicitudesVM> ObtenerMisSolicitudes(int usuarioId);
        ActaAdopcionVM ObtenerDatosActa(int solicitudId);
    }
}
