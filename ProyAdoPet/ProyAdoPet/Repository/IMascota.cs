using ProyAdoPet.Models;

namespace ProyAdoPet.Repository
{
    public interface IMascota: IConsulta<Mascota>
    {
        IEnumerable<Estado> ListarEstado();
        bool Registrar(Mascota objeto);
        Mascota Obtener(int id);
        bool Actualizar(Mascota obj);
        bool Eliminar(int id);

        // HU-09: Filtrar mascotas
        IEnumerable<Mascota> Filtrar(string? nombre, string? edad, int? estadoId);
    }
}
