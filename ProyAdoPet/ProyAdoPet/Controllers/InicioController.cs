using Microsoft.AspNetCore.Mvc;
using ProyAdoPet.Models;
using ProyAdoPet.Services;

namespace ProyAdoPet.Controllers
{
    public class InicioController : Controller
    {
        MascotaService _mascota;
        public InicioController(MascotaService mascota)
        {
            _mascota = mascota;
        }


        public async Task<IActionResult> Mascotas()
        {

            var lista = await Task.Run(() => _mascota.MascotasDisponibles());

            if (lista == null || !lista.Any())
            {
                TempData["Mensaje"] = "No hay mascotas disponibles en este momento";
                return View(new List<Mascota>());
            }

            return View(lista);

        }

        [HttpGet("Detalle/{id}")]
        public IActionResult DetalleMascota(int id)
        {
            if (id <= 0)
            {
                return RedirectToAction("Mascotas");
            }

            var mascota = _mascota.ObtenerMascota(id);

            if (mascota == null)
            {
                return NotFound();
            }

            return View(mascota);
        }


    }
}
