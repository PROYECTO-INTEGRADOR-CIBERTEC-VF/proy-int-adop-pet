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

        [HttpPost("Postular")]
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


        [HttpGet("Bandeja/Detalle")]
        [Authorize(Roles = RolesConstantes.Administrador)]
        public IActionResult Evaluar(int id)
        {
            var modelo = _solicitudService.ObtenerDetalleSolicitud(id);
            if (modelo == null) return NotFound();

            return View(modelo);
        }

        [HttpPost("Bandeja/Detalle/Cita")]
        [Authorize(Roles = RolesConstantes.Administrador)]
        public IActionResult ProgramarCita(CitaAdopcion cita)
        {
            if (cita.SolicitudId == 0) return BadRequest();

            bool exito = _solicitudService.ProgramarEntrevista(cita);

            if (exito)
            {
                TempData["MensajeExito"] = "La cita ha sido programada y el estado se actualizó a 'Citado'.";
            }
            else
            {
                TempData["MensajeError"] = "No se pudo programar la cita. Verifique que la fecha sea válida.";
            }

            return RedirectToAction("Evaluar", new { id = cita.SolicitudId });
        }

        [HttpPost("Bandeja/Detalle/Aprobar")]
        [Authorize(Roles = RolesConstantes.Administrador)]
        public IActionResult ConfirmarAdopcion(int SolicitudId, string Observaciones)
        {
            if (SolicitudId <= 0) return BadRequest();
            var contrato = _solicitudService.AprobarYGenerarContrato(SolicitudId, Observaciones);

            if (contrato != null)
            {
                return View("FichaAdopcion", contrato);
            }
            else
            {
                TempData["MensajeError"] = "Hubo un error técnico al intentar finalizar la adopción.";
                return RedirectToAction("Evaluar", new { id = SolicitudId });
            }

        }

        [HttpGet("Bandeja/Contrato")]
        [Authorize(Roles = RolesConstantes.Administrador)]
        public IActionResult VerContrato(int id)
        {
            var contrato = _solicitudService.ObtenerContratoPorSolicitud(id);

            if (contrato == null)
            {
                TempData["MensajeError"] = "No se encontró un contrato para esta solicitud.";
                return RedirectToAction("Bandeja");
            }

            return View("FichaAdopcion", contrato);
        }

        [HttpPost("Rechazar")]
        [Authorize(Roles = RolesConstantes.Administrador)]
        public IActionResult Rechazar(int SolicitudId)
        {
            if (SolicitudId <= 0) return BadRequest();

            bool exito = _solicitudService.ProcesarRechazo(SolicitudId);

            if (exito)
            {
                TempData["MensajeExito"] = "La solicitud ha sido rechazada y la mascota está disponible nuevamente.";
            }
            else
            {
                TempData["MensajeError"] = "Hubo un error al procesar el rechazo.";
            }

            return RedirectToAction("Evaluar", new { id = SolicitudId });
        }
    }
}
