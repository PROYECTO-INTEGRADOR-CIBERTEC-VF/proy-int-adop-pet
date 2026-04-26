using ProyAdoPet.Models;

namespace ProyAdoPet.Repository
{
    public interface ISolicitudAdopcion
    {
        bool Registrar(SolicitudAdopcion solicitud);
    }
}
