using ProyAdoPet.Models;
using System.Collections.Generic;

namespace ProyAdoPet.Repository
{
    public interface IMascota
    {
        List<Mascota> Listado();

        Mascota ObtenerMascotaPorId(int id);

        List<Mascota> FiltrarMascotas(int? edad, string tipo, string tamano);
    }
}
