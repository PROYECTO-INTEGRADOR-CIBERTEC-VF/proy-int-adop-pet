using Microsoft.AspNetCore.Mvc;

namespace ProyAdoPet.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Dashboard() => View();
    }
}
