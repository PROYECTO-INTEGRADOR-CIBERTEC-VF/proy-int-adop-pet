using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyAdoPet.Constants;
using ProyAdoPet.Services;

namespace ProyAdoPet.Controllers
{
    [Route("Seguimiento")]
    public class SeguimientoController : Controller
    {
        private readonly SeguimientoService _seguimientoService;
        public SeguimientoController(SeguimientoService seguimiento)
        {
            _seguimientoService = seguimiento;
        }

        [HttpGet("Lista")]
        [Authorize(Roles = RolesConstantes.Administrador)]
        public IActionResult Lista()
        {
            var lista = _seguimientoService.ListarAdopcionesEnSeguimiento();
            return View(lista);
        }

    }
}
