using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyAdoPet.Constants;
using ProyAdoPet.Models;
using ProyAdoPet.Services;
using System.Security.Claims;

namespace ProyAdoPet.Controllers
{
    [Authorize]
    [Route("Adopcion")]
    public class SolicitudController : Controller
    {
        private readonly SolicitudService _solicitudService;
        private readonly MascotaService _mascotaService;

        public SolicitudController(SolicitudService solicitudService, MascotaService mascotaService)
        {
            _solicitudService = solicitudService;
            _mascotaService = mascotaService;
        }

        //HU-16: SOLICITUD DE ADOPCION
        [HttpGet("Formulario/{mascotaId}")]
        public IActionResult Postular(int mascotaId)
        {
            var mascota = _mascotaService.ObtenerMascota(mascotaId);
            if (mascota == null) return NotFound();

            ViewBag.MascotaNombre = mascota.Nombre;
            ViewBag.MascotaFoto = mascota.FotoMascota;

            var modelo = new SolicitudAdopcion { MascotaId = mascotaId };
            return View(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Postular(SolicitudAdopcion solicitud)
        {
            string idUsuarioStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                 ?? User.FindFirst("IdUsuario")?.Value;
            solicitud.UsuarioId = int.Parse(idUsuarioStr);

            solicitud.FechaCreacion = DateTime.Now;

            if (ModelState.IsValid)
            {
                bool exito = _solicitudService.EnviarSolicitud(solicitud);
                if (exito)
                {
                    return RedirectToAction("Confirmacion", new
                    {
                        nombre = solicitud.NombreCompleto,
                        fecha = solicitud.FechaCreacion.ToString("dd/MM/yyyy HH:mm")
                    });
                }
            }

            TempData["MensajeError"] = "Hubo un problema al enviar la solicitud.";
            return View(solicitud);
        }

        [HttpGet]
        public IActionResult Confirmacion(string nombre, string fecha)
        {
            ViewBag.Nombre = nombre;
            ViewBag.Fecha = fecha;
            return View();
        }

        //FUNCIONES PARA ADMIN
        [HttpGet("Bandeja")]
        [Authorize(Roles = RolesConstantes.Administrador)]
        public IActionResult Bandeja()
        {
            var solicitudes = _solicitudService.ObtenerBandejaAdmin();
            return View(solicitudes);
        }
    }
}
