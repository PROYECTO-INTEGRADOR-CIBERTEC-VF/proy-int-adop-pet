using ProyAdoPet.ViewModel;

namespace ProyAdoPet.Repository
{
    public interface ISeguimiento
    {
        List<SeguimientoListaVM> ListarAdopcionesEnSeguimiento();
        List<SeguimientoItemVM> ListarControlesPorSolicitud(int solicitudId);
    }
}
