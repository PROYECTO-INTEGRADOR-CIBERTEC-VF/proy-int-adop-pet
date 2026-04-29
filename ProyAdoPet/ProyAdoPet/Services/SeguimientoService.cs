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

        public List<SeguimientoListaVM> ListarAdopcionesEnSeguimiento()
        {
            return _seguimiento.ListarAdopcionesEnSeguimiento();
        }
    }
}
