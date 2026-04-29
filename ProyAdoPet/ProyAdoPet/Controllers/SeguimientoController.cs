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

        [HttpGet("Detalle")]
        [Authorize(Roles = RolesConstantes.Administrador)]
        public IActionResult VerSeguimiento(int id)
        {
            var model = _seguimientoService.ObtenerHistorialSeguimiento(id);

            if (model == null) return NotFound();

            return View(model);
        }

        //programar visita
        [HttpPost("ProgramarVisita")]
        [Authorize(Roles = RolesConstantes.Administrador)]
        [ValidateAntiForgeryToken] 
        public IActionResult ProgramarVisita(int SolicitudId, DateTime FechaProgramada, string TipoControl, string Responsable, string ObservacionInicial)
        {
            try
            {
                if (FechaProgramada == DateTime.MinValue)
                {
                    ModelState.AddModelError("", "La fecha programada no es válida.");
                    return RedirectToAction("VerSeguimiento", new { id = SolicitudId });
                }

                bool exito = _seguimientoService.ProgramarNuevaVisita(SolicitudId, FechaProgramada, TipoControl, Responsable, ObservacionInicial);

                if (exito)
                {
                    TempData["Mensaje"] = "Visita programada correctamente.";
                    return RedirectToAction("VerSeguimiento", new { id = SolicitudId });
                }
                else
                {
                    TempData["Error"] = "No se pudo programar la visita.";
                    return RedirectToAction("VerSeguimiento", new { id = SolicitudId });
                }
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
    }
}
