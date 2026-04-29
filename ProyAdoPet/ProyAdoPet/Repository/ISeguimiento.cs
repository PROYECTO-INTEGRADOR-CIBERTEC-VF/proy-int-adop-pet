using ProyAdoPet.ViewModel;

namespace ProyAdoPet.Repository
{
    public interface ISeguimiento
    {
        List<SeguimientoListaVM> ListarAdopcionesEnSeguimiento();
        List<SeguimientoItemVM> ListarControlesPorSolicitud(int solicitudId);

        //programar visista
        bool ProgramarVisita(int solicitudId, DateTime fecha, string tipo, string responsable, string obs);
        //registrar datos post visita
        bool RegistrarResultadoVisita(int id, DateTime fecha, string resultado, string comentarios, string foto);


        //listado para usuario adoptante
        List<MisMascotasVM> ListarMisMascotasAdoptadas(int usuarioId);
    }
}
