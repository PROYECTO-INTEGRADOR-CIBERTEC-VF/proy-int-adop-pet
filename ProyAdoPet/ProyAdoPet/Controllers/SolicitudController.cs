using Microsoft.AspNetCore.Mvc;
using ProyAdoPet.Repository;

namespace ProyAdoPet.Controllers
{
    public class SolicitudController : Controller
    {
        private readonly ISolicitud _dao;

        public SolicitudController(ISolicitud dao)
        {
            _dao = dao;
        }

        // GET
        public IActionResult Solicitar(int idMascota)
        {
            ViewBag.IdMascota = idMascota;
            return View();
        }

        // POST
        [HttpPost]
        public IActionResult Solicitar(int idMascota, string mensaje)
        {
            int idUsuario = Convert.ToInt32(HttpContext.Session.GetInt32("IdUsuario"));

            if (string.IsNullOrEmpty(mensaje))
            {
                ViewBag.Error = "Ingrese un mensaje";
                return View();
            }

            bool ok = _dao.Solicitar(idUsuario, idMascota, mensaje);

            if (ok)
                ViewBag.Mensaje = "Solicitud enviada correctamente";
            else
                ViewBag.Error = "Error al enviar la solicitud";

            return View();
        }
    }
}