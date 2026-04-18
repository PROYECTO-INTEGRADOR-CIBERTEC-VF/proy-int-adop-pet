using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyAdoPet.Constants;

namespace ProyAdoPet.Controllers
{
    //solo es accesible por un usuario administrador
    [Authorize(Roles = RolesConstantes.Administrador)]
    public class AdminController : Controller
    {
        public IActionResult Dashboard() => View();
    }
}
