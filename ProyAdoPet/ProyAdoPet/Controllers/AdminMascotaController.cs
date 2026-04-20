using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyAdoPet.Constants;
using ProyAdoPet.Models;
using ProyAdoPet.Services;

namespace ProyAdoPet.Controllers
{
    [Authorize(Roles = RolesConstantes.Administrador)]
    [Route("Admin/Mascotas")]
    public class AdminMascotaController : Controller
    {
        private readonly MascotaService _mascotaService;
        public AdminMascotaController(MascotaService mascotaService)
        {
            _mascotaService = mascotaService;
        }

        [HttpGet("")]
        public IActionResult ListadoMascotas()
        {
            var mascotas = _mascotaService.MascotasDisponibles();

            if (mascotas == null)
            {
                return View(new List<Mascota>());
            }

            return View(mascotas);
        }

        [HttpGet("Registrar")]
        public IActionResult RegistrarMascota()
        {
            var listaEstados =  _mascotaService.EstadosMascota();
            ViewBag.Estados = new SelectList(listaEstados, "Id", "EstadoNombre");
            return View();
        }
    }
}
