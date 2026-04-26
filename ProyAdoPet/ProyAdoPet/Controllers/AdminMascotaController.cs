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
        private readonly IWebHostEnvironment _hostEnvironment;

        public AdminMascotaController(MascotaService mascotaService, IWebHostEnvironment hostEnvironment)
        {
            _mascotaService = mascotaService;
            _hostEnvironment = hostEnvironment;
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

        [HttpPost("Registrar")]
        public async Task<IActionResult> RegistrarMascota(Mascota objeto, IFormFile FotoArchivo)
        {
            ModelState.Remove("Id");
            if (ModelState.IsValid)
            {

                if (FotoArchivo != null && FotoArchivo.Length > 0)
                {
                    string nombreUnico = Guid.NewGuid().ToString() + Path.GetExtension(FotoArchivo.FileName);

                    string rutaCarpeta = Path.Combine(_hostEnvironment.WebRootPath, "fotos");

                    if (!Directory.Exists(rutaCarpeta)) Directory.CreateDirectory(rutaCarpeta);

                    string rutaCompleta = Path.Combine(rutaCarpeta, nombreUnico);
                    using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                    {
                        await FotoArchivo.CopyToAsync(stream);
                    }

                    objeto.FotoMascota = nombreUnico;
                }
                else
                {
                    objeto.FotoMascota = "sin-foto.jpg";
                }
                bool resultado = await _mascotaService.RegistrarMascota(objeto);

                if (resultado)
                {
                    TempData["MensajeExito"] = "¡Mascota registrada correctamente!";
                    return RedirectToAction("ListadoMascotas");
                }
                else
                {
                    ViewBag.Error = "Ocurrió un error al guardar en la base de datos.";
                }
            }

            var listaEstados = _mascotaService.EstadosMascota();
            ViewBag.Estados = new SelectList(listaEstados, "Id", "EstadoNombre");
            return View(objeto);
        }
    
        // HU07 → EDITAR GET
        [HttpGet("Editar")]
        public IActionResult Editar(int id)
        {
            var mascota = _mascotaService.ObtenerMascota(id);

            var listaEstados = _mascotaService.EstadosMascota();
            ViewBag.Estados = new SelectList(listaEstados, "Id", "EstadoNombre");

            return View(mascota);
        }

        //  HU07 → EDITAR POST
        [HttpPost("Editar")]
        public async Task<IActionResult> Editar(Mascota obj, IFormFile? FotoArchivo)
        {
            if (ModelState.IsValid)
            {
                var mascotaActual =  _mascotaService.ObtenerMascota(obj.Id);

                if (FotoArchivo != null && FotoArchivo.Length > 0)
                {
                    if (!string.IsNullOrEmpty(mascotaActual.FotoMascota))
                    {
                        string rutaFotoVieja = Path.Combine(_hostEnvironment.WebRootPath, "fotos", mascotaActual.FotoMascota);

                        if (System.IO.File.Exists(rutaFotoVieja))
                        {
                            System.IO.File.Delete(rutaFotoVieja); //borramos foto vieja
                        }
                    }

                    string nombreUnico = Guid.NewGuid().ToString() + Path.GetExtension(FotoArchivo.FileName);
                    string rutaCarpeta = Path.Combine(_hostEnvironment.WebRootPath, "fotos");
                    if (!Directory.Exists(rutaCarpeta)) Directory.CreateDirectory(rutaCarpeta);
                    string rutaCompleta = Path.Combine(rutaCarpeta, nombreUnico);

                    using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                    {
                        await FotoArchivo.CopyToAsync(stream);
                    }

                    obj.FotoMascota = nombreUnico;
                }
                else
                {
                    obj.FotoMascota = mascotaActual.FotoMascota;
                }

                bool ok = await _mascotaService.ActualizarMascota(obj);

                if (ok)
                {
                    TempData["MensajeExito"] = "Mascota actualizada correctamente";
                    return RedirectToAction("ListadoMascotas", "AdminMascota");
                }

                ViewBag.Error = "Error al actualizar la mascota en la base de datos";
            }

            var listaEstados = _mascotaService.EstadosMascota();
            ViewBag.Estados = new SelectList(listaEstados, "Id", "EstadoNombre");

            return View(obj);
        }

        //HU-09: ELIMINAR MASCOTA METODO POST
        [HttpPost]
        public IActionResult Eliminar(int id)
        {
            bool seElimino = _mascotaService.EliminarMascota(id);

            if (seElimino)
            {
                TempData["MensajeExito"] = "La mascota se quitó del listado correctamente.";
            }
            else
            {
                TempData["MensajeError"] = "No se pudo eliminar. Verifica que la mascota no esté ya adoptada.";
            }

            return RedirectToAction("ListadoMascotas");
        }
    }
}

