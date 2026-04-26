using ProyAdoPet.Models;
using ProyAdoPet.Repository;

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

    }
}
