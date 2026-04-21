using ProyAdoPet.Models;

namespace ProyAdoPet.Repository
{
    public interface ISolicitud
    {
        public interface ISolicitud
        {
            bool Solicitar(int idUsuario, int idMascota, string mensaje);
            IEnumerable<SolicitudAdopcion> ListarPorUsuario(int idUsuario);
        }
    }
}