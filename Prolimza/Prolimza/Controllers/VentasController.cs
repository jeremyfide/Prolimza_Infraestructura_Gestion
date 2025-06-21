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
            return View(await _context.Ventas.ToListAsync());
        }

        // GET: Ventas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var venta = await _context.Ventas
                .Include(v => v.HistorialesEstadoVenta)
                .FirstOrDefaultAsync(m => m.IdVenta == id);
            if (venta == null) return NotFound();

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

            return View(venta);
        }

        // GET: Ventas/Create
        public IActionResult Create()
        {
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "Nombre");
            return View();
        }

        // POST: Ventas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdVenta,FechaVenta,IdUsuario")] Venta venta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(venta);
                await _context.SaveChangesAsync();

                var estadoPendiente = await _context.EstadosVenta.FirstOrDefaultAsync(e => e.Descripcion == "Pendiente");
                if (estadoPendiente != null)
                {
                    var historial = new HistorialEstadoVenta
                    {
                        IdVenta = venta.IdVenta,
                        IdEstadoVenta = estadoPendiente.IdEstadoVenta,
                        FechaEstado = DateTime.Now
                    };
                    _context.HistorialesEstadoVenta.Add(historial);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "Nombre");
            return View(venta);
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    // Actualizar manualmente las propiedades permitidas
                    ventaOriginal.FechaVenta = venta.FechaVenta;
                    ventaOriginal.IdUsuario = venta.IdUsuario;

                    // Auditoria
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
                        IdUsuario = null // O coloca ventaOriginal.IdUsuario si la alerta es individual
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
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VentaExists(int id)
        {
            return _context.Ventas.Any(e => e.IdVenta == id);
        }
        // GET: Ventas/Cancelar/5
        public async Task<IActionResult> Cancelar(int? id)
        {
            if (id == null) return NotFound();

            var venta = await _context.Ventas
                .Include(v => v.HistorialesEstadoVenta)
                .FirstOrDefaultAsync(v => v.IdVenta == id);

            if (venta == null) return NotFound();

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
                TempData["Error"] = $"La orden no puede ser cancelada porque su estado es '{estadoDescripcion}'.";
                return RedirectToAction(nameof(Details), new { id });
            }

            return View(venta);
        }
        // POST: Ventas/Cancelar/5
        [HttpPost, ActionName("Cancelar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelarConfirmed(int id)
        {
            var venta = await _context.Ventas
                .Include(v => v.HistorialesEstadoVenta)
                .FirstOrDefaultAsync(v => v.IdVenta == id);

            if (venta == null) return NotFound();

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
                TempData["Error"] = $"La orden no puede ser cancelada porque su estado es '{estadoDescripcion}'.";
                return RedirectToAction(nameof(Details), new { id });
            }

            // Agregar nuevo estado 'Cancelado' en historial
            var estadoCancelado = await _context.EstadosVenta.FirstOrDefaultAsync(e => e.Descripcion == "Cancelado");
            if (estadoCancelado == null)
            {
                TempData["Error"] = "El estado 'Cancelado' no existe.";
                return RedirectToAction(nameof(Details), new { id });
            }

            var nuevoHistorial = new HistorialEstadoVenta
            {
                IdVenta = venta.IdVenta,
                IdEstadoVenta = estadoCancelado.IdEstadoVenta,
                FechaEstado = DateTime.Now
            };

            _context.HistorialesEstadoVenta.Add(nuevoHistorial);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Orden cancelada correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
