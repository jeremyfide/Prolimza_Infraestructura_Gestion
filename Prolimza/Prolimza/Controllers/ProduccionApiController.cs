using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prolimza.Models;
using System.Linq;

namespace Prolimza.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProduccionApiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProduccionApiController(ApplicationDbContext context)
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

        [HttpGet("reporte-produccion")]
        public async Task<IActionResult> ObtenerReporteProduccion(DateTime? fechaInicio, DateTime? fechaFin, string? tipoProducto)
        {
            var ventasBase = await (from dv in _context.DetallesVenta
                                    join v in _context.Ventas on dv.IdVenta equals v.IdVenta
                                    join p in _context.Productos on dv.IdProducto equals p.IdProducto
                                    where (!fechaInicio.HasValue || v.FechaVenta >= fechaInicio)
                                       && (!fechaFin.HasValue || v.FechaVenta <= fechaFin)
                                    select new
                                    {
                                        dv.IdVenta,
                                        v.FechaVenta,
                                        p.NombreProducto,
                                        dv.Cantidad,
                                        p.IdProducto
                                    }).ToListAsync();

            var recetasDict = await _context.Recetas
                .ToDictionaryAsync(r => r.IdProducto);

            var materiasDict = await _context.MateriasReceta
                .Join(_context.MateriasPrimas,
                      mr => mr.IdMateriaPrima,
                      mp => mp.IdMateriaPrima,
                      (mr, mp) => new
                      {
                          mr.IdReceta,
                          MateriaPrima = mp.Nombre,
                          mr.Cantidad
                      })
                .GroupBy(x => x.IdReceta)
                .ToDictionaryAsync(g => g.Key, g => g.ToList());

            var resultado = ventasBase.Select(v =>
            {
                recetasDict.TryGetValue(v.IdProducto, out var receta);
                string tipo = receta != null ? "Producido" : "Comprado";

                string? nombreMateriaPrima = null;
                decimal? cantidadMateriaUsada = null;

                if (receta != null && materiasDict.TryGetValue(receta.IdReceta, out var materias))
                {
                    var primera = materias.FirstOrDefault();
                    if (primera != null)
                    {
                        nombreMateriaPrima = primera.MateriaPrima;
                        cantidadMateriaUsada = primera.Cantidad * v.Cantidad;
                    }
                }

                return new
                {
                    v.IdVenta,
                    FechaVenta = v.FechaVenta,
                    NombreProducto = v.NombreProducto,
                    TipoProducto = tipo,
                    NombreMateriaPrima = nombreMateriaPrima,
                    CantidadMateriaPrimaUsada = cantidadMateriaUsada,
                    CantidadProductoVendido = v.Cantidad
                };
            })
            .Where(r => string.IsNullOrEmpty(tipoProducto) || r.TipoProducto == tipoProducto)
            .OrderBy(r => r.IdVenta)
            .ThenBy(r => r.NombreProducto)
            .ToList();


            return Ok(resultado);

        }

        [HttpGet("recetas-mas-utilizadas")]
        public async Task<IActionResult> RecetasMasUtilizadas(DateTime? fechaInicio, DateTime? fechaFin, string? tipoProducto)
        {
            var ventas = await _context.DetallesVenta
                .Include(dv => dv.Venta)
                .Where(dv => (!fechaInicio.HasValue || dv.Venta.FechaVenta >= fechaInicio)
                          && (!fechaFin.HasValue || dv.Venta.FechaVenta <= fechaFin))
                .ToListAsync();

            var recetas = await _context.Recetas
                .Include(r => r.Producto)
                .ToDictionaryAsync(r => r.IdProducto);

            var recetasUsadas = ventas
                .Where(v =>
                {
                    var esProducido = recetas.ContainsKey(v.IdProducto);
                    return string.IsNullOrEmpty(tipoProducto)
                           || (tipoProducto == "Producido" && esProducido)
                           || (tipoProducto == "Comprado" && !esProducido);
                })
                .GroupBy(v => recetas.ContainsKey(v.IdProducto) ? recetas[v.IdProducto].Nombre : "Desconocida")
                .Select(g => new
                {
                    Receta = g.Key,
                    Total = g.Sum(v => v.Cantidad)
                })
                .OrderByDescending(r => r.Total)
                .ToList();

            return Ok(recetasUsadas);
        }


        [HttpGet("materia-por-dia")]
        public async Task<IActionResult> MateriaPorDia(DateTime? fechaInicio, DateTime? fechaFin, string? tipoProducto)
        {
            var ventas = await _context.DetallesVenta
                .Include(dv => dv.Venta)
                .Where(dv => (!fechaInicio.HasValue || dv.Venta.FechaVenta >= fechaInicio)
                          && (!fechaFin.HasValue || dv.Venta.FechaVenta <= fechaFin))
                .ToListAsync();

            var recetasDict = await _context.Recetas
                .ToDictionaryAsync(r => r.IdProducto);

            var materiasDict = await _context.MateriasReceta
                .Include(mr => mr.MateriaPrima)
                .GroupBy(mr => mr.IdReceta)
                .ToDictionaryAsync(g => g.Key, g => g.ToList());

            var materiasPorVenta = ventas
                .Where(v =>
                {
                    var esProducido = recetasDict.ContainsKey(v.IdProducto);
                    return string.IsNullOrEmpty(tipoProducto)
                           || (tipoProducto == "Producido" && esProducido)
                           || (tipoProducto == "Comprado" && !esProducido);
                })
                .Where(v => recetasDict.ContainsKey(v.IdProducto))
                .SelectMany<DetalleVenta, (string Fecha, string Materia, decimal Cantidad)>(v =>
                {
                    var receta = recetasDict[v.IdProducto];
                    var fecha = v.Venta.FechaVenta.Date;

                    if (!materiasDict.TryGetValue(receta.IdReceta, out var materias))
                        return Enumerable.Empty<(string Fecha, string Materia, decimal Cantidad)>();

                    return materias.Select(mr => (
                        Fecha: fecha.ToString("yyyy-MM-dd"),
                        Materia: mr.MateriaPrima.Nombre,
                        Cantidad: (decimal)mr.Cantidad * v.Cantidad
                    ));
                });

            var datos = materiasPorVenta
                .GroupBy(x => new { x.Fecha, x.Materia })
                .Select(g => new
                {
                    Fecha = g.Key.Fecha,
                    Materia = g.Key.Materia,
                    Total = g.Sum(x => x.Cantidad)
                })
                .OrderBy(r => r.Fecha)
                .ToList();

            return Ok(datos);
        }

        [HttpGet("eficiencia-materia-prima")]
        public async Task<IActionResult> EficienciaMateriaPrimaPorDia(DateTime? fechaInicio, DateTime? fechaFin, string? tipoProducto)
        {
            const decimal desperdicio = 0.07m; // 7%

            var ventas = await _context.DetallesVenta
                .Include(dv => dv.Venta)
                .Where(dv => (!fechaInicio.HasValue || dv.Venta.FechaVenta >= fechaInicio)
                          && (!fechaFin.HasValue || dv.Venta.FechaVenta <= fechaFin))
                .ToListAsync();

            var recetasDict = await _context.Recetas
                .ToDictionaryAsync(r => r.IdProducto);

            var materiasDict = await _context.MateriasReceta
                .Include(mr => mr.MateriaPrima)
                .GroupBy(mr => mr.IdReceta)
                .ToDictionaryAsync(g => g.Key, g => g.ToList());

            var materiasPorVenta = ventas
                .Where(v =>
                {
                    var esProducido = recetasDict.ContainsKey(v.IdProducto);
                    return string.IsNullOrEmpty(tipoProducto)
                           || (tipoProducto == "Producido" && esProducido)
                           || (tipoProducto == "Comprado" && !esProducido);
                })
                .Where(v => recetasDict.ContainsKey(v.IdProducto))
                .SelectMany<DetalleVenta, (DateTime Fecha, string Materia, decimal Cantidad)>(v =>
                {
                    var receta = recetasDict[v.IdProducto];
                    var fecha = v.Venta.FechaVenta.Date;

                    if (!materiasDict.TryGetValue(receta.IdReceta, out var materias))
                        return Enumerable.Empty<(DateTime Fecha, string Materia, decimal Cantidad)>();

                    return materias.Select(mr => (
                        Fecha: fecha,
                        Materia: mr.MateriaPrima.Nombre,
                        Cantidad: (decimal)mr.Cantidad * v.Cantidad
                    ));
                });



            // Agrupar por día y materia prima, y calcular pérdidas
            var datosPorDia = materiasPorVenta
                .GroupBy(x => new { x.Fecha, x.Materia })
                .Select(g => new
                {
                    Fecha = g.Key.Fecha,
                    Materia = g.Key.Materia,
                    CantidadUsada = g.Sum(x => x.Cantidad),
                    Perdida = Math.Round(g.Sum(x => x.Cantidad) * desperdicio, 4)
                })
                .GroupBy(x => x.Fecha)
                .Select(g =>
                {
                    var totalPerdida = g.Sum(x => x.Perdida);
                    var totalMaterias = g.Count();

                    return new
                    {
                        Fecha = g.Key.ToString("yyyy-MM-dd"),
                        Detalle = g.Select(x => new
                        {
                            x.Materia,
                            x.CantidadUsada,
                            x.Perdida,
                            Eficiencia = totalMaterias == 1
                                ? 100
                                : totalPerdida > 0
                                    ? Math.Round((1 - (x.Perdida / totalPerdida)) * 100, 2)
                                    : 100
                        }).OrderByDescending(x => x.Eficiencia).ToList()
                    };

                })
                .ToList();

            return Ok(datosPorDia);
        }


    }
}
