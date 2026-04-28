using ProyAdoPet.Models;
using System.Collections.Generic;

namespace ProyAdoPet.Repository
{
    public interface IMascota
    {
        List<Mascota> Listado();
        Mascota ObtenerMascotaPorId(int id);
        List<Mascota> FiltrarMascotas(string? nombre, string? edad, int? estadoId);

        IEnumerable<Estado> ListarEstado();
        bool Registrar(Mascota objeto);
        Mascota Obtener(int id);
        bool Actualizar(Mascota obj);
        bool Eliminar(int id);
    }
}
