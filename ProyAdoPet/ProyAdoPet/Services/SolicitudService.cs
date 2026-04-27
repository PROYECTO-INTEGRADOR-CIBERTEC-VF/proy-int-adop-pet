using ProyAdoPet.Models;
using ProyAdoPet.Repository;
using ProyAdoPet.ViewModel;

namespace ProyAdoPet.Services
{
    public class SolicitudService
    {
        ISolicitudAdopcion _solicitud;

        public SolicitudService(ISolicitudAdopcion solicitud)
        {
            _solicitud = solicitud;
        }

        //HU-16: REGISTRO DE SOLICITUD DE ADOPCION
        public bool EnviarSolicitud(SolicitudAdopcion solicitud)
        {
            if (solicitud.MascotaId <= 0 || solicitud.UsuarioId <= 0) return false;
            return _solicitud.Registrar(solicitud);
        }

        public bool VerificarExistencia(int mascotaId, int usuarioId)
        {
            return _solicitud.YaTieneSolicitud(mascotaId, usuarioId);
        }

        //FUNCIONES PARA EL ADMIN
        public List<SolicitudAdopcionVM> ObtenerBandejaAdmin()
        {
            return _solicitud.ListarParaAdmin();
        }

    }
}
