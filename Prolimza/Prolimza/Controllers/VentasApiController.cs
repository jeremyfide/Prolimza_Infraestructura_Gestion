using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prolimza.Models;
using System.Globalization;

namespace Prolimza.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasApiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VentasApiController(ApplicationDbContext context)
        {
            _context = context;
        }
        private async Task<List<Venta>> ObtenerVentasValidas(DateTime fechaInicio, DateTime fechaFin)
        {
            var estadoCanceladoId = await _context.EstadosVenta
                .Where(e => e.Descripcion == "Cancelado")
                .Select(e => e.IdEstadoVenta)
                .FirstOrDefaultAsync();

            var ventas = await _context.Ventas
                .Where(v => v.FechaVenta >= fechaInicio && v.FechaVenta <= fechaFin)
                .Include(v => v.HistorialesEstadoVenta.OrderByDescending(h => h.FechaEstado))
                .Include(v => v.Usuario)
                .Include(v => v.DetallesVenta!)
                    .ThenInclude(dv => dv.Producto)
                .ToListAsync();

            return ventas
                .Where(v => v.HistorialesEstadoVenta.FirstOrDefault()?.IdEstadoVenta != estadoCanceladoId)
                .ToList();
        }

        [HttpGet("ventas-por-semana")]
        public async Task<IActionResult> ObtenerVentasPorSemana(DateTime fechaInicio, DateTime fechaFin)
        {
            var ventas = await ObtenerVentasValidas(fechaInicio, fechaFin);

            var cultura = CultureInfo.InvariantCulture;
            var calendario = cultura.Calendar;

            var datos = ventas
                .GroupBy(v => calendario.GetWeekOfYear(
                    v.FechaVenta,
                    CalendarWeekRule.FirstFourDayWeek,
                    DayOfWeek.Monday))
                .Select(g => new
                {
                    Semana = g.Key,
                    TotalVentas = g.Count()
                })
                .OrderBy(x => x.Semana)
                .ToList();

            return Ok(datos);
        }

        [HttpGet("ventas-por-semana-por-usuario")]
        public async Task<IActionResult> ObtenerVentasPorSemanaPorUsuario(DateTime fechaInicio, DateTime fechaFin)
        {
            var ventas = await ObtenerVentasValidas(fechaInicio, fechaFin);

            var cultura = CultureInfo.InvariantCulture;
            var calendario = cultura.Calendar;

            var datos = ventas
                .GroupBy(v => calendario.GetWeekOfYear(
                    v.FechaVenta,
                    CalendarWeekRule.FirstFourDayWeek,
                    DayOfWeek.Monday))
                .Select(g => new
                {
                    Semana = g.Key,
                    Usuarios = g.GroupBy(v => v.Usuario?.Nombre ?? "Desconocido")
                                .Select(u => new
                                {
                                    Nombre = u.Key,
                                    TotalVentas = u.Count()
                                })
                                .OrderBy(u => u.Nombre)
                                .ToList()
                })
                .OrderBy(x => x.Semana)
                .ToList();

            return Ok(datos);
        }

        [HttpGet("ventas-por-producto")]
        public async Task<IActionResult> ObtenerVentasPorProducto(DateTime fechaInicio, DateTime fechaFin)
        {
            var ventas = await ObtenerVentasValidas(fechaInicio, fechaFin);

            var datos = ventas
                .SelectMany(v => v.DetallesVenta ?? new List<DetalleVenta>())
                .Where(dv => dv.Producto != null)
                .GroupBy(dv => dv.Producto!.NombreProducto)
                .Select(g => new
                {
                    Producto = g.Key,
                    Total = g.Sum(d => d.Cantidad)
                })
                .OrderByDescending(g => g.Total)
                .ToList();

            return Ok(datos);
        }

        [HttpGet("ventas-por-usuario-producto")]
        public async Task<IActionResult> ObtenerVentasPorUsuarioYProducto(DateTime fechaInicio, DateTime fechaFin)
        {
            var ventas = await ObtenerVentasValidas(fechaInicio, fechaFin);

            var datos = ventas
                .SelectMany(v => v.DetallesVenta ?? new List<DetalleVenta>(), (venta, detalle) => new
                {
                    Usuario = venta.Usuario?.Nombre ?? "Desconocido",
                    Producto = detalle.Producto?.NombreProducto ?? "Sin Nombre",
                    detalle.Cantidad
                })
                .GroupBy(x => new { x.Usuario, x.Producto })
                .Select(g => new
                {
                    g.Key.Usuario,
                    g.Key.Producto,
                    Total = g.Sum(x => x.Cantidad)
                })
                .ToList();

            return Ok(datos);
        }
    }
}
