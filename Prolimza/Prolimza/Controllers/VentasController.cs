using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Prolimza.Models;

namespace Prolimza.Controllers
{
    public class VentasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VentasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Ventas
        public async Task<IActionResult> Index()
        {
            var ventas = await _context.Ventas
                .Include(v => v.Usuario)
                .ToListAsync();

            if (TempData["SuccessCancelar"] != null)
                ViewBag.SuccessCancelar = TempData["SuccessCancelar"];
            if (TempData["ErrorCancelar"] != null)
                ViewBag.ErrorCancelar = TempData["ErrorCancelar"];

            return View(ventas);
        }

        // GET: Ventas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var venta = await _context.Ventas
                .Include(v => v.DetallesVenta)
                    .ThenInclude(dv => dv.Producto)
                .Include(v => v.Usuario)
                .Include(v => v.HistorialesEstadoVenta)
                    .ThenInclude(h => h.EstadoVenta)
                .FirstOrDefaultAsync(v => v.IdVenta == id);

            if (venta == null)
                return NotFound();

            venta.DetallesVenta ??= new List<DetalleVenta>();
            venta.HistorialesEstadoVenta ??= new List<HistorialEstadoVenta>();

            return PartialView(venta);
        }

        // GET: Ventas/Create
        public IActionResult Create()
        {
            var productos = _context.Productos
                .Select(p => new {
                    IdProducto = p.IdProducto,
                    NombreProducto = p.NombreProducto,
                    Cantidad = p.Cantidad
                }).ToList();

            ViewBag.StockProductos = productos;
            ViewBag.Productos = new SelectList(productos, "IdProducto", "NombreProducto");

            return View(new Venta());
        }

        // POST: Ventas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Venta model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var productos = model.DetallesVenta ?? new List<DetalleVenta>();
            var idUsuarioClaim = User.FindFirst("IdUsuario");

            if (idUsuarioClaim == null)
            {
                ViewBag.Error = "Usuario no autenticado.";
                return View(model);
            }

            var estadoPendiente = await _context.EstadosVenta
                .FirstOrDefaultAsync(e => e.Descripcion == "Pendiente");

            if (estadoPendiente == null)
            {
                ViewBag.Error = "Estado pendiente no definido en la base de datos.";
                return View(model);
            }

            int usuarioId = int.Parse(idUsuarioClaim.Value);
            DateTime fecha = DateTime.Now;

            var venta = new Venta
            {
                IdUsuario = usuarioId,
                CodigoIngreso = model.CodigoIngreso,
                FechaVenta = fecha,
                DetallesVenta = new List<DetalleVenta>(),
                HistorialesEstadoVenta = new List<HistorialEstadoVenta>
                    {
                        new HistorialEstadoVenta
                        {
                            IdEstadoVenta = estadoPendiente.IdEstadoVenta,
                            FechaEstado = fecha
                        }
                    }
            };

            foreach (var detalle in productos)
            {
                var producto = await _context.Productos.FindAsync(detalle.IdProducto);
                if (producto == null) continue;

                if (producto.Cantidad < detalle.Cantidad)
                {
                    ViewBag.StockError = $"No hay suficiente stock de {producto.NombreProducto}.";

                    var productosStock = _context.Productos
                        .Select(p => new {
                            IdProducto = p.IdProducto,
                            NombreProducto = p.NombreProducto,
                            Cantidad = p.Cantidad
                        }).ToList();

                    ViewBag.StockProductos = productosStock;
                    ViewBag.Productos = new SelectList(productosStock, "IdProducto", "NombreProducto");

                    return View(model);
                }

                producto.Cantidad -= detalle.Cantidad;

                venta.DetallesVenta.Add(new DetalleVenta
                {
                    IdProducto = detalle.IdProducto,
                    Cantidad = detalle.Cantidad
                });
            }

            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Ventas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var venta = await _context.Ventas
                .Include(v => v.HistorialesEstadoVenta)
                .FirstOrDefaultAsync(v => v.IdVenta == id);

            if (venta == null)
            {
                return NotFound();
            }
            var ultimoEstadoHistorial = venta.HistorialesEstadoVenta?
                    .OrderByDescending(h => h.FechaEstado)
                    .FirstOrDefault();

            string estadoDescripcion = "Desconocido";
            if (ultimoEstadoHistorial != null)
            {
                var estado = await _context.EstadosVenta.FindAsync(ultimoEstadoHistorial.IdEstadoVenta);
                estadoDescripcion = estado?.Descripcion ?? "Desconocido";
            }

            ViewData["EstadoActual"] = estadoDescripcion;
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "Nombre", venta.IdUsuario);
            return View(venta);
        }

        // POST: Ventas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdVenta,FechaVenta,IdUsuario")] Venta venta)
        {
            if (id != venta.IdVenta)
                return NotFound();

            var ventaOriginal = await _context.Ventas
                .Include(v => v.HistorialesEstadoVenta)
                .FirstOrDefaultAsync(v => v.IdVenta == id);

            if (ventaOriginal == null)
                return NotFound();

            var ultimoEstado = ventaOriginal.HistorialesEstadoVenta?
                .OrderByDescending(h => h.FechaEstado)
                .FirstOrDefault();

            string estadoDescripcion = "Desconocido";
            if (ultimoEstado != null)
            {
                var estado = await _context.EstadosVenta.FindAsync(ultimoEstado.IdEstadoVenta);
                estadoDescripcion = estado?.Descripcion ?? "Desconocido";
            }

            if (estadoDescripcion != "Pendiente")
            {
                ModelState.AddModelError(string.Empty, "No se puede editar la orden porque su estado no es 'Pendiente'.");
                ViewData["EstadoActual"] = estadoDescripcion;
                ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "Nombre", venta.IdUsuario);
                return View(venta);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ventaOriginal.FechaVenta = venta.FechaVenta;
                    ventaOriginal.IdUsuario = venta.IdUsuario;

                    // Auditoría
                    var log = $"Venta Id {ventaOriginal.IdVenta} modificada. Fecha: {ventaOriginal.FechaVenta}, UsuarioId: {ventaOriginal.IdUsuario}";
                    var auditoria = new Auditoria
                    {
                        Log = log,
                        TipoEvento = "Modificación de Venta",
                        FechaEvento = DateTime.Now,
                        IdUsuario = ventaOriginal.IdUsuario
                    };
                    _context.Auditorias.Add(auditoria);

                    // Alerta del sistema
                    var alerta = new Alerta
                    {
                        Tipo = "Sistema",
                        Descripcion = $"La venta con ID {ventaOriginal.IdVenta} fue modificada por el usuario con ID {ventaOriginal.IdUsuario}.",
                        FechaAlerta = DateTime.Now,
                        Estado = "Pendiente",
                        IdUsuario = null
                    };
                    _context.Alertas.Add(alerta);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VentaExists(venta.IdVenta)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["EstadoActual"] = estadoDescripcion;
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "Nombre", venta.IdUsuario);
            return View(venta);
        }

        // GET: Ventas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas
                .FirstOrDefaultAsync(m => m.IdVenta == id);
            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
        }

        // POST: Ventas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venta = await _context.Ventas.FindAsync(id);
            if (venta != null)
            {
                _context.Ventas.Remove(venta);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool VentaExists(int id)
        {
            return _context.Ventas.Any(e => e.IdVenta == id);
        }

        // POST: Ventas/Cancelar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancelar(int id, string motivo)
        {
            var venta = await _context.Ventas
                .Include(v => v.HistorialesEstadoVenta)
                .Include(v => v.DetallesVenta)
                    .ThenInclude(dv => dv.Producto)
                .FirstOrDefaultAsync(v => v.IdVenta == id);

            if (venta == null)
            {
                TempData["ErrorCancelar"] = "La venta no existe.";
                return RedirectToAction(nameof(Index));
            }

            var ultimoEstadoHistorial = venta.HistorialesEstadoVenta?
                .OrderByDescending(h => h.FechaEstado)
                .FirstOrDefault();

            string estadoDescripcion = "Desconocido";
            if (ultimoEstadoHistorial != null)
            {
                var estado = await _context.EstadosVenta.FindAsync(ultimoEstadoHistorial.IdEstadoVenta);
                estadoDescripcion = estado?.Descripcion ?? "Desconocido";
            }

            if (estadoDescripcion != "Pendiente")
            {
                TempData["ErrorCancelar"] = $"La orden no puede ser cancelada porque su estado es '{estadoDescripcion}'.";
                return RedirectToAction(nameof(Index));
            }

            var estadoCancelado = await _context.EstadosVenta.FirstOrDefaultAsync(e => e.Descripcion == "Cancelado");
            if (estadoCancelado == null)
            {
                TempData["ErrorCancelar"] = "El estado 'Cancelado' no está definido en la base de datos.";
                return RedirectToAction(nameof(Index));
            }

            // Volver productos al inventario
            foreach (var detalle in venta.DetallesVenta)
            {
                var producto = detalle.Producto;
                if (producto != null)
                {
                    producto.Cantidad += detalle.Cantidad;
                    _context.Productos.Update(producto);
                }
            }

            var nuevoHistorial = new HistorialEstadoVenta
            {
                IdVenta = venta.IdVenta,
                IdEstadoVenta = estadoCancelado.IdEstadoVenta,
                FechaEstado = DateTime.Now
            };

            _context.HistorialesEstadoVenta.Add(nuevoHistorial);

            // Obtener usuario actual
            var idUsuarioClaim = User.FindFirst("IdUsuario");
            int usuarioId = idUsuarioClaim != null ? int.Parse(idUsuarioClaim.Value) : 0;

            // Guardar auditoría
            var auditoria = new Auditoria
            {
                Log = $"Cancelación de venta Id {venta.IdVenta}. Motivo: {motivo}",
                TipoEvento = "Cancelación de Venta",
                FechaEvento = DateTime.Now,
                IdUsuario = usuarioId
            };
            _context.Auditorias.Add(auditoria);

            await _context.SaveChangesAsync();

            TempData["SuccessCancelar"] = "Orden cancelada correctamente.";
            return RedirectToAction(nameof(Index));
        }

    }
}