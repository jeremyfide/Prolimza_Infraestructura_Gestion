using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Prolimza.Models;

namespace Prolimza.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            /* Verifica si hay sesión activa
           if (HttpContext.Session.GetInt32("UsuarioId") == null)
            {
                // Redirige al login si no hay sesión
                return RedirectToAction("Login", "Auth");
            }

            // Si hay sesión, puede mostrar el home normal
            ViewBag.NombreUsuario = HttpContext.Session.GetString("UsuarioNombre");
            ViewBag.Rol = HttpContext.Session.GetString("RolNombre");*/

            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
