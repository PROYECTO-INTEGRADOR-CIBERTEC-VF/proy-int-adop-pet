using ProyAdoPet.Models;

namespace ProyAdoPet.Repository
{
    public interface IMascota: IConsulta<Mascota>
    {
        //listado de estados de mascotas
        IEnumerable<Estado> ListarEstado();
    }
}
