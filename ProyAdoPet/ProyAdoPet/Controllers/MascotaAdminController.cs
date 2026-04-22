using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyAdoPet.Models;
using ProyAdoPet.Repository;

namespace ProyAdoPet.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MascotaAdminController : Controller
    {
        private readonly IMascotaAdmin _dao;

        public MascotaAdminController(IMascotaAdmin dao)
        {
            _dao = dao;
        }

        public IActionResult Index()
        {
            return View(_dao.Listado());
        }

        // GET
        public IActionResult Editar(int id)
        {
            var mascota = _dao.ObtenerPorId(id);

            if (mascota == null)
                return RedirectToAction("Index");

            return View(mascota);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Mascota obj)
        {
            if (ModelState.IsValid)
            {
                bool ok = _dao.Actualizar(obj);

                if (ok)
                    return RedirectToAction("Index");
            }

            return View(obj);
        }
    }
}

