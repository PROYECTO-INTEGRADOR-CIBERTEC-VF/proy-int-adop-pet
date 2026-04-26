using Microsoft.AspNetCore.Mvc;
using ProyAdoPet.Models;
using ProyAdoPet.Services;

namespace ProyAdoPet.Controllers
{
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
        [HttpGet]
        public IActionResult Postular(int mascotaId)
        {
            var mascota = _mascotaService.ObtenerMascota(mascotaId);
            if (mascota == null) return NotFound();

            ViewBag.MascotaNombre = mascota.Nombre;
            ViewBag.MascotaFoto = mascota.FotoMascota;

            var modelo = new SolicitudAdopcion { MascotaId = mascotaId };
            return View(modelo);
        }


    }
}
