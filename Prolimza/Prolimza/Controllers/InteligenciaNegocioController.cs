// Controlador: InteligenciaNegocioController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prolimza.Models;
using System.Globalization;

namespace Prolimza.Controllers
{
    public class InteligenciaNegocioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InteligenciaNegocioController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult ReporteVentas()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ReporteVentas(DateTime fechaInicio, DateTime fechaFin)
        {
            if (fechaInicio > fechaFin)
            {
                ModelState.AddModelError("", "El rango de fechas no es válido.");
                return View();
            }

            var ventas = await _context.Ventas
                .Include(v => v.Usuario)
                .Include(v => v.DetallesVenta)
                .Where(v => v.FechaVenta >= fechaInicio && v.FechaVenta <= fechaFin)
                .ToListAsync();

            ViewData["Ventas"] = ventas;
            ViewData["FechaInicio"] = fechaInicio.ToString("yyyy-MM-dd");
            ViewData["FechaFin"] = fechaFin.ToString("yyyy-MM-dd");

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GraficoEmpleadoMasVentas()
        {
            var data = await _context.Ventas
                .GroupBy(v => v.Usuario.Nombre)
                .Select(g => new { Empleado = g.Key, Total = g.Count() })
                .ToListAsync();

            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GraficoProductoMasVendido()
        {
            var data = await _context.DetallesVenta
                .Include(dv => dv.Producto)
                .GroupBy(dv => dv.Producto.NombreProducto)
                .Select(g => new { Producto = g.Key, Total = g.Sum(x => x.Cantidad) })
                .ToListAsync();

            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GraficoVentasPorPeriodo(string periodo)
        {
            var query = _context.Ventas.AsQueryable();

            var data = periodo switch
            {
                "dia" => await query
                    .GroupBy(v => v.FechaVenta.Date)
                    .Select(g => new { Periodo = g.Key.ToString("yyyy-MM-dd"), Total = g.Count() })
                    .ToListAsync(),
                "semana" => await query
                    .GroupBy(v => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(v.FechaVenta, CalendarWeekRule.FirstDay, DayOfWeek.Monday))
                    .Select(g => new { Periodo = "Semana " + g.Key, Total = g.Count() })
                    .ToListAsync(),
                _ => await query
                    .GroupBy(v => new { v.FechaVenta.Year, v.FechaVenta.Month })
                    .Select(g => new { Periodo = $"{g.Key.Year}-{g.Key.Month:D2}", Total = g.Count() })
                    .ToListAsync()
            };

            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GraficoEstadosVenta()
        {
            var data = await _context.HistorialesEstadoVenta
                .Include(h => h.EstadoVenta)
                .GroupBy(h => h.EstadoVenta.Descripcion)
                .Select(g => new { Estado = g.Key, Total = g.Count() })
                .ToListAsync();

            return Json(data);
        }
    }
}
