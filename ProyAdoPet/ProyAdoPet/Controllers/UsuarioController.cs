using Microsoft.AspNetCore.Mvc;
using ProyAdoPet.Models;
using ProyAdoPet.Services;

namespace ProyAdoPet.Controllers
{
    public class UsuarioController : Controller
    {
        UsuarioService _usuario;

        public UsuarioController(UsuarioService usuario)
        {
            _usuario = usuario;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(Usuario user)
        {
            //validar data annotations
            if (!ModelState.IsValid) return View(user);

            string resultado = await Task.Run(() => _usuario.Registrar(user));

            if (resultado == "OK")
            {
                TempData["Mensaje"] = "¡Cuenta creada con éxito! Ya puedes iniciar sesión.";
                return RedirectToAction("Mascotas", "Inicio");
            }
            else
            {
                ViewBag.Error = resultado;
                return View(user);
            }
        }
    }
}
