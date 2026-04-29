using ProyAdoPet.Repository;
using ProyAdoPet.ViewModel;

namespace ProyAdoPet.Services
{
    public class SeguimientoService
    {
        ISeguimiento _seguimiento;

        public SeguimientoService(ISeguimiento seguimiento)
        {
            _seguimiento = seguimiento;
        }

        public List<SeguimientoListaVM> ObtenerListaAdopciones()
        {
            return _seguimiento.ListarAdopcionesEnSeguimiento();
        }
    }
}
