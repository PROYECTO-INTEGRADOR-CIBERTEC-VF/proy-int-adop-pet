using ProyAdoPet.Models;

namespace ProyAdoPet.Repository
{
    public interface ISolicitudAdopcion
    {
        public bool YaTieneSolicitud(int mascotaId, int usuarioId);
        bool Registrar(SolicitudAdopcion solicitud);
    }
}
