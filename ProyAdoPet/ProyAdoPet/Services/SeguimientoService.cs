using ProyAdoPet.DAO;
using ProyAdoPet.Repository;
using ProyAdoPet.ViewModel;

namespace ProyAdoPet.Services
{
    public class SeguimientoService
    {
        ISeguimiento _seguimiento;
        IAdopcion _adopcionDAO;

        public SeguimientoService(ISeguimiento seguimiento, IAdopcion adopcion)
        {
            _seguimiento = seguimiento;
            _adopcionDAO = adopcion;
        }

        //LISTA DE ADOPCIONES EN SEGUIMIENTO
        public List<SeguimientoListaVM> ListarAdopcionesEnSeguimiento()
        {
            return _seguimiento.ListarAdopcionesEnSeguimiento();
        }

        //EL DETALLE DE CADA SEGUIMIENTO DE ADOPCION
        public DetalleSeguimientoVM ObtenerHistorialSeguimiento(int solicitudId)
        {

            var datosBase = _adopcionDAO.ObtenerDatosActa(solicitudId);

            if (datosBase == null) return null;

            var historial = _seguimiento.ListarControlesPorSolicitud(solicitudId);

            return new DetalleSeguimientoVM
            {
                SolicitudId = solicitudId,
                CodigoContrato = datosBase.CodigoContrato,
                MascotaNombre = datosBase.MascotaNombre,
                AdoptanteNombre = datosBase.AdoptanteNombre,
                Controles = historial
            };
        }

        //programar visita
        public bool ProgramarNuevaVisita(int solicitudId, DateTime fecha, string tipo, string responsable, string obs)
        {
            return _seguimiento.ProgramarVisita(solicitudId, fecha, tipo, responsable, obs);
        }

        //registrar datos de seguimiento
        public bool RegistrarResultadoVisita(int id, DateTime fecha, string resultado, string comentarios, string foto)
        {
            return _seguimiento.RegistrarResultadoVisita(id, fecha, resultado, comentarios, foto);
        }


        //listado para usuario adoptante
        public List<MisMascotasVM> ListarMisMascotasAdoptadas(int usuarioId)
        {
            return _seguimiento.ListarMisMascotasAdoptadas(usuarioId);
        }
    }
}
