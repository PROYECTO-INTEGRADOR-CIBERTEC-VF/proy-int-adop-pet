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

        [HttpPost("CompletarVisita")]
        [Authorize(Roles = RolesConstantes.Administrador)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompletarVisita(int SeguimientoId, int SolicitudId, DateTime FechaRealizada, string Resultado, string Comentarios, IFormFile FotoFile)
        {
            try
            {
                string nombreFoto = null;
                if (FotoFile != null && FotoFile.Length > 0)
                {
                    string extension = Path.GetExtension(FotoFile.FileName);
                    nombreFoto = Guid.NewGuid().ToString() + extension;

                    string rutaCarpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "adop", "seguimientos");

                    if (!Directory.Exists(rutaCarpeta))
                        Directory.CreateDirectory(rutaCarpeta);

                    string rutaCompleta = Path.Combine(rutaCarpeta, nombreFoto);

                    using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                    {
                        await FotoFile.CopyToAsync(stream);
                    }
                }

                bool exito = _seguimientoService.RegistrarResultadoVisita(SeguimientoId, FechaRealizada, Resultado, Comentarios, nombreFoto);

                if (exito)
                {
                    TempData["Mensaje"] = "Visita finalizada con éxito.";
                }
                else
                {
                    TempData["Error"] = "No se pudo actualizar el registro.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error técnico: " + ex.Message;
            }

            return RedirectToAction("VerSeguimiento", new { id = SolicitudId });
        }
    }
}
