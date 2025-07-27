using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Prolimza.Services.IA;
using Prolimza.Models;
using System.Linq;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Prolimza.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class PrediccionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrediccionController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var productos = _context.Productos.ToList();
            ViewBag.Productos = new SelectList(productos, "IdProducto", "NombreProducto");

            return View(0);
        }

        [HttpPost]
        public IActionResult Predecir(int idProducto)
        {
            var servicio = new PrediccionService(_context);
            var resultados = servicio.PredecirVentas(idProducto);
            var producto = _context.Productos.FirstOrDefault(p => p.IdProducto == idProducto)?.NombreProducto;

            ViewBag.Producto = producto;
            ViewBag.Resultados = resultados;
            ViewBag.Productos = new SelectList(_context.Productos.ToList(), "IdProducto", "NombreProducto", idProducto);

            return View("Index", idProducto);
        }

        [HttpPost]
        [Route("Prediccion/PredecirAsync")]
        public IActionResult PredecirAsync(int idProducto)
        {
            var servicio = new PrediccionService(_context);
            var resultados = servicio.PredecirVentas(idProducto);
            var producto = _context.Productos.FirstOrDefault(p => p.IdProducto == idProducto)?.NombreProducto;

            ViewBag.Producto = producto;
            ViewBag.Resultados = resultados;

            return PartialView("_ResultadoPrediccion", idProducto);
        }


    }
}
