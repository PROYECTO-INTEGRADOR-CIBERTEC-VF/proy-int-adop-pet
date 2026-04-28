using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyAdoPet.Services;
using Rotativa.AspNetCore;
using System.Security.Claims;

namespace ProyAdoPet.Controllers
{
    [Authorize]
    [Route("Adopcion")]
    public class AdopcionController : Controller
    {
        private readonly AdopcionService _adopcionService;
        private readonly SolicitudService _solicitudService;

        public AdopcionController(AdopcionService adopcionService, SolicitudService solicitudService)
        {
            _adopcionService = adopcionService;
            _solicitudService = solicitudService;
        }

        [HttpGet("Solicitudes")]
        [Authorize]
        public IActionResult MisSolicitudes()
        {
            var claimId = User.Claims.FirstOrDefault(c => c.Type == "IdUsuario")?.Value;

            if (string.IsNullOrEmpty(claimId))
            {
                return RedirectToAction("Login", "LogIn");
            }

            int usuarioId = int.Parse(claimId);

            var solicitudes = _adopcionService.ObtenerMisSolicitudes(usuarioId);

            return View(solicitudes);
        }

        [HttpGet("Detalle")]
        [Authorize]
        public IActionResult DetalleSolicitud(int id)
        {
            int usuarioId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "IdUsuario")?.Value);


            var solicitud = _solicitudService.ObtenerDetalleSolicitud(id);

            if (solicitud == null || solicitud.UsuarioId != usuarioId)
            {
                return RedirectToAction("MisSolicitudes");
            }

            return View(solicitud);
        }

        [HttpGet("Descargar")]
        [Authorize]
        public IActionResult DescargarActa(int id)
        {
            int usuarioId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "IdUsuario")?.Value);

            var acta = _adopcionService.ObtenerDatosActa(id);

            if (acta == null) return NotFound();

            return new ViewAsPdf("_ActaPDF", acta)
            {
                FileName = $"Acta_{acta.MascotaNombre}_{acta.CodigoContrato}.pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
            };
        }

    }
}
