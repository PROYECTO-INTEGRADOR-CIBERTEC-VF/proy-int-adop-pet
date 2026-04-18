using Microsoft.AspNetCore.Mvc;

namespace ProyAdoPet.Controllers
{
    public class AdminMascotaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
