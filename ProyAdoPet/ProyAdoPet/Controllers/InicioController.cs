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

            if (lista == null || !lista.Any())
            {
                TempData["Mensaje"] = "No hay mascotas disponibles en este momento";
                return View(new List<Mascota>());
            }

            return View(lista);

        }


    }
}
