using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyAdoPet.Models;
using ProyAdoPet.Services;

namespace ProyAdoPet.Controllers
{
    public class UsuarioController : Controller
    {
        UsuarioService _usuario;
        SeguimientoService _seguimiento;

        public UsuarioController(UsuarioService usuario, SeguimientoService seguimiento)
        {
            _usuario = usuario;
            _seguimiento = seguimiento;
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
                return RedirectToAction("Index", "Inicio");
            }
            else
            {
                ViewBag.Error = resultado;
                return View(user);
            }
        }

        [HttpGet("MisAdopciones")]
        [Authorize]
        public IActionResult MisAdopciones()
        {
            var claimId = User.Claims.FirstOrDefault(c => c.Type == "IdUsuario");

            if (claimId == null) return RedirectToAction("Login", "LogIn");
            int idUsuario = int.Parse(claimId.Value);
            var lista = _seguimiento.ListarMisMascotasAdoptadas(idUsuario);

            return View(lista);
        }
    }
}
