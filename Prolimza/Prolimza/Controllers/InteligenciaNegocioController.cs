﻿// Controlador: InteligenciaNegocioController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
        public async Task<IActionResult> ReporteVentas()
        {
            var estadoPendiente = await _context.EstadosVenta
                .Where(e => e.Descripcion == "Pendiente")
                .Select(e => e.IdEstadoVenta)
                .FirstOrDefaultAsync();

            var ventasPendientes = await _context.Ventas
                .Include(v => v.Usuario)
                .Include(v => v.HistorialesEstadoVenta.OrderByDescending(h => h.FechaEstado))
                    .ThenInclude(h => h.EstadoVenta)
                .Where(v =>
                    v.HistorialesEstadoVenta
                        .OrderByDescending(h => h.FechaEstado)
                        .FirstOrDefault().IdEstadoVenta == estadoPendiente
                )
                .ToListAsync();

            // Diccionario: IdVenta => tiempo pendiente como string
            var tiempos = ventasPendientes.ToDictionary(
                v => v.IdVenta,
                v =>
                {
                    var fechaEstado = v.HistorialesEstadoVenta.OrderByDescending(h => h.FechaEstado).FirstOrDefault()?.FechaEstado;
                    return CalcularTiempoDesde(fechaEstado);
                }
            );

            ViewBag.TiemposPendientes = tiempos;

            return View(ventasPendientes);

        }

        [HttpGet]
        public async Task<IActionResult> ReporteRecetas()
        {
            return View();
        }

        private string CalcularTiempoDesde(DateTime? fecha)
        {
            if (fecha == null) return "Desconocido";

            var tiempo = DateTime.Now - fecha.Value;

            if (tiempo.TotalDays >= 1)
                return $"{(int)tiempo.TotalDays} días";
            if (tiempo.TotalHours >= 1)
                return $"{(int)tiempo.TotalHours} horas";
            return $"{(int)tiempo.TotalMinutes} minutos";
        }

        /*
        [HttpGet]
        public async Task<IActionResult> ReporteRecetas(DateTime? fechaInicio, DateTime? fechaFin, string? tipoProducto)
        {
            string sql = @"
        SELECT 
            dv.idVenta,
            v.fechaVenta,
            p.nombreProducto,
            CASE 
                WHEN r.idReceta IS NOT NULL THEN 'Producido'
                ELSE 'Comprado'
            END AS tipoProducto,
            mp.nombre AS nombreMateriaPrima,
            mr.cantidad * dv.cantidad AS cantidadMateriaPrimaUsada,
            dv.cantidad AS cantidadProductoVendido
        FROM 
            detalleVenta dv
        JOIN venta v ON v.idVenta = dv.idVenta
        JOIN producto p ON p.idProducto = dv.idProducto
        LEFT JOIN receta r ON r.idProducto = p.idProducto
        LEFT JOIN materiaReceta mr ON mr.idReceta = r.idReceta
        LEFT JOIN materiaPrima mp ON mp.idMateriaPrima = mr.idMateriaPrima
        WHERE 
            (@fechaInicio IS NULL OR v.fechaVenta >= @fechaInicio) AND
            (@fechaFin IS NULL OR v.fechaVenta <= @fechaFin) AND
            (@tipoProducto IS NULL OR 
                (@tipoProducto = 'Producido' AND r.idReceta IS NOT NULL) OR
                (@tipoProducto = 'Comprado' AND r.idReceta IS NULL)
            )
        ORDER BY dv.idVenta, p.nombreProducto;
    ";

            var reporte = await _context.ReporteUsoMateriaPrimaViewModel
                .FromSqlRaw(sql,
                    new SqlParameter("@fechaInicio", (object?)fechaInicio ?? DBNull.Value),
                    new SqlParameter("@fechaFin", (object?)fechaFin ?? DBNull.Value),
                    new SqlParameter("@tipoProducto", (object?)tipoProducto ?? DBNull.Value)
                )
                .ToListAsync();

            ViewData["FechaInicio"] = fechaInicio?.ToString("yyyy-MM-dd");
            ViewData["FechaFin"] = fechaFin?.ToString("yyyy-MM-dd");
            ViewData["TipoProducto"] = tipoProducto;

            return View(reporte);
        }


        [HttpGet]
        public async Task<IActionResult> GetRecetasPorDia(DateTime? fechaInicio, DateTime? fechaFin)
        {
            // Si no se especifican fechas, usar los últimos 7 días
            if (!fechaInicio.HasValue || !fechaFin.HasValue)
            {
                fechaFin = DateTime.Today.AddDays(1); // para incluir todo el día de hoy
                fechaInicio = fechaFin.Value.AddDays(-7);
            }

            string sql = @"
        SELECT 
            CONVERT(date, v.fechaVenta) AS Fecha,
            r.idReceta,
            r.nombre AS NombreReceta,
            SUM(dv.cantidad) AS CantidadProductosVendidos
        FROM 
            detalleVenta dv
        JOIN venta v ON v.idVenta = dv.idVenta
        JOIN producto p ON p.idProducto = dv.idProducto
        JOIN receta r ON r.idProducto = p.idProducto
        WHERE 
            v.fechaVenta >= @fechaInicio AND v.fechaVenta < @fechaFin
        GROUP BY 
            CONVERT(date, v.fechaVenta), r.idReceta, r.nombre
        ORDER BY Fecha, r.idReceta;";

            var data = await _context.RecetasPorDiaViewModel
                .FromSqlRaw(sql,
                    new SqlParameter("@fechaInicio", fechaInicio),
                    new SqlParameter("@fechaFin", fechaFin)
                )
                .ToListAsync();

            return Json(data);
        }





        [HttpGet]
public async Task<IActionResult> GetTopProductos(DateTime? fechaInicio, DateTime? fechaFin)
{
    string sql = @"
        SELECT TOP 3
            p.nombreProducto AS NombreProducto,
            SUM(dv.cantidad) AS CantidadVendida
        FROM 
            detalleVenta dv
        JOIN venta v ON v.idVenta = dv.idVenta
        JOIN producto p ON p.idProducto = dv.idProducto
        JOIN receta r ON r.idProducto = p.idProducto
        WHERE 
            (@fechaInicio IS NULL OR v.fechaVenta >= @fechaInicio) AND
            (@fechaFin IS NULL OR v.fechaVenta <= @fechaFin)
        GROUP BY p.nombreProducto
        ORDER BY CantidadVendida DESC";

    var data = await _context.TopProductosViewModel
        .FromSqlRaw(sql,
            new SqlParameter("@fechaInicio", (object?)fechaInicio ?? DBNull.Value),
            new SqlParameter("@fechaFin", (object?)fechaFin ?? DBNull.Value)
        )
        .ToListAsync();

    return Json(data);
}
        */


    }
}
