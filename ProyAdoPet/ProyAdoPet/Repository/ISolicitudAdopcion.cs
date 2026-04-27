using ProyAdoPet.Models;
using ProyAdoPet.ViewModel;

namespace ProyAdoPet.Repository
{
    public interface ISolicitudAdopcion
    {
        public bool YaTieneSolicitud(int mascotaId, int usuarioId);
        bool Registrar(SolicitudAdopcion solicitud);

        //METODOS PARA EL ADMIN
        List<SolicitudAdopcionVM> ListarParaAdmin();
    }
}
