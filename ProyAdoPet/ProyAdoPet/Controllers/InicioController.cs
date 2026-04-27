using Microsoft.AspNetCore.Mvc;
using ProyAdoPet.Models;
using ProyAdoPet.Services;
using ProyAdoPet.ViewModel;

namespace ProyAdoPet.Controllers
{
    public class InicioController : Controller
    {
        MascotaService _mascota;
        SolicitudService _solcitud;
        public InicioController(MascotaService mascota, SolicitudService solicitud)
        {
            _mascota = mascota;
            _solcitud = solicitud;
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

        // HU-09: Filtrar mascotas
        [HttpGet]
        public IActionResult FiltrarMascotas(string? nombre, string? edad, int? estadoId)
        {
            var vm = new FiltroMascotaVM
            {
                Nombre = nombre,
                Edad = edad,
                EstadoId = estadoId,
                Estados = _mascota.EstadosMascota()
            };

            bool hayFiltros = !string.IsNullOrWhiteSpace(nombre)
                           || !string.IsNullOrWhiteSpace(edad)
                           || estadoId.HasValue;

            if (hayFiltros)
            {
                vm.Resultados = _mascota.FiltrarMascotas(nombre, edad, estadoId);
                vm.BusquedaRealizada = true;
            }

            return View(vm);
        }

        [HttpGet("Detalle/{id}")]
        public IActionResult DetalleMascota(int id)
        {
            if (id <= 0)
            {
                return RedirectToAction("Mascotas");
            }

            var mascota = _mascota.ObtenerMascota(id);
            if (mascota == null) return NotFound();

            bool yaPostulo = false;

            if (User.Identity.IsAuthenticated)
            {
                int usuarioId = int.Parse(User.FindFirst("IdUsuario").Value);

                yaPostulo = _solcitud.VerificarExistencia(id, usuarioId);
            }

            ViewBag.YaPostulo = yaPostulo;
            return View(mascota);
        }


    }
}
