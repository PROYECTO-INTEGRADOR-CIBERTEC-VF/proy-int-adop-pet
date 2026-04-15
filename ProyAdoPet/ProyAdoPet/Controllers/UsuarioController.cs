using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ProyAdoPet.Models;
using ProyAdoPet.Services;
using System.Security.Claims;

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

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string correo, string clave)
        {
            var usuarioEncontrado = _usuario.ValidarUsuario(correo, clave);

            if (usuarioEncontrado != null)
            {
                //lista de claims - datos del usuario que viajan
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuarioEncontrado.Nombre),
                    new Claim(ClaimTypes.Email, usuarioEncontrado.Correo),
                    new Claim(ClaimTypes.Role, usuarioEncontrado.IdRol.ToString()),
                    new Claim("IdUsuario", usuarioEncontrado.IdUsuario.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                //firmar la cookie
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Mascotas", "Inicio");
            }

            ViewBag.Error = "Credenciales incorrectas.";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Mascotas", "Inicio");
        }
    }
}
