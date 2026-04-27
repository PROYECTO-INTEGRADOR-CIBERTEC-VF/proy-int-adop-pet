using Microsoft.AspNetCore.Mvc;
using ProyAdoPet.Models;
using ProyAdoPet.Services;
using System.Linq;

namespace ProyAdoPet.Controllers
{
    public class InicioController : Controller
    {
        private readonly MascotaService _mascota;

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
        public IActionResult Index(int? edad, string tipo, string tamano)
        {
            var lista = _mascota.FiltrarMascotas(edad, tipo, tamano);

            if (lista == null || !lista.Any())
            {
                ViewBag.Mensaje = "No existen mascotas con esos criterios";
            }

            return View(lista);
        }

        public IActionResult Detalle(int id)
        {
            var obj = _mascota.ObtenerMascotaPorId(id);

            if (obj == null)
                return RedirectToAction("Index");

            return View(obj);
        }

        public IActionResult Limpiar()
        {
            return RedirectToAction("Index");
        }
    }
}
