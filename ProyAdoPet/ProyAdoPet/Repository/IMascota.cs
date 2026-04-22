using ProyAdoPet.Models;

namespace ProyAdoPet.Repository
{
    public interface IMascota: IConsulta<Mascota>
    {
        //listado de estados de mascotas
        IEnumerable<Estado> ListarEstado();
        bool Registrar(Mascota objeto);
        Mascota Obtener(int id);
        bool Actualizar(Mascota obj);
    }
}
