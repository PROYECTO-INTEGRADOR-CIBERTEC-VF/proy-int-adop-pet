using ProyAdoPet.Models;

namespace ProyAdoPet.Repository
{
    public interface IMascotaAdmin
    {
        IEnumerable<Mascota> Listado();
        Mascota ObtenerPorId(int id);
        bool Actualizar(Mascota obj);
    }
}
