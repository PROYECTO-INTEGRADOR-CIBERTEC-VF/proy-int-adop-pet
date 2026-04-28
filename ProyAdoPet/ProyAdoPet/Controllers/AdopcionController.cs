using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyAdoPet.Services;
using System.Security.Claims;

namespace ProyAdoPet.Controllers
{
    [Authorize]
    [Route("Adopcion")]
    public class AdopcionController : Controller
    {
        private readonly AdopcionService _adopcionService;

        public AdopcionController(AdopcionService adopcionService)
        {
            _adopcionService = adopcionService;
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
    }
}
