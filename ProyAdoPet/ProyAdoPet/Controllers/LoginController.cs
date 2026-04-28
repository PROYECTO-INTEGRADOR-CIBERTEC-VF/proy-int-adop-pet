using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ProyAdoPet.Services;
using ProyAdoPet.Constants;
using Microsoft.AspNetCore.Authorization;

namespace ProyAdoPet.Controllers
{
    public class LoginController : Controller
    {
        UsuarioService _usuario;

        public LoginController(UsuarioService usuario)
        {
            _usuario = usuario;
        }

        [HttpGet]
        public IActionResult LogIn() => View();

        [HttpPost]
        public async Task<IActionResult> LogIn(string correo, string clave)
        {
            var usuarioEncontrado = _usuario.ValidarUsuario(correo, clave);

            if (usuarioEncontrado != null)
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuarioEncontrado.Nombre),
            new Claim(ClaimTypes.Role, usuarioEncontrado.IdRol.ToString()),
            new Claim("IdUsuario", usuarioEncontrado.IdUsuario.ToString())
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                //logica de redireccion
                if (usuarioEncontrado.IdRol.ToString() == RolesConstantes.Administrador)
                {
                    return RedirectToAction("Dashboard", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Inicio");
                }
            }

            ViewBag.Error = "Credenciales incorrectas.";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Inicio");
        }

        [AllowAnonymous] //todos puede ver esta pagina
        public IActionResult AccesoDenegado() => View();
    }
}
