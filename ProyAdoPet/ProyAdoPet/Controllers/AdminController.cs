using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyAdoPet.Constants;
using ProyAdoPet.Models;
using ProyAdoPet.Services;

namespace ProyAdoPet.Controllers
{
    //solo es accesible por un usuario administrador
    [Authorize(Roles = RolesConstantes.Administrador)]
    public class AdminController : Controller

    {
        private readonly UsuarioService _usuarioService;
        public AdminController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public IActionResult Dashboard() => View();

        [HttpGet]
        public IActionResult Registrar() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registrar(Usuario oUsuario)
        {
            if (!ModelState.IsValid)
            {
                return View(oUsuario);
            }

            string mensaje = _usuarioService.RegistrarAdministrador(oUsuario);
            if (mensaje.Equals("OK"))
            {
                TempData["MensajeExito"] = "¡Nuevo co-administrador registrado correctamente!";
                return RedirectToAction("Registrar", "Admin");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View(oUsuario);
            }
        }
    }
}
