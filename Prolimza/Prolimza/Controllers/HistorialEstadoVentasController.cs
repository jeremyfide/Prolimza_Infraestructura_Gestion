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
    public class HistorialEstadoVentasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HistorialEstadoVentasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HistorialEstadoVentas
        public async Task<IActionResult> Index()
        {
            return View(await _context.HistorialesEstadoVenta.ToListAsync());
        }

        // GET: HistorialEstadoVentas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historialEstadoVenta = await _context.HistorialesEstadoVenta
                .FirstOrDefaultAsync(m => m.IdHistorialEstadoVenta == id);
            if (historialEstadoVenta == null)
            {
                return NotFound();
            }

            return View(historialEstadoVenta);
        }

        // GET: HistorialEstadoVentas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HistorialEstadoVentas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdHistorialEstadoVenta,IdVenta,IdEstadoVenta,FechaEstado")] HistorialEstadoVenta historialEstadoVenta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(historialEstadoVenta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(historialEstadoVenta);
        }

        // GET: HistorialEstadoVentas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historialEstadoVenta = await _context.HistorialesEstadoVenta.FindAsync(id);
            if (historialEstadoVenta == null)
            {
                return NotFound();
            }
            return View(historialEstadoVenta);
        }

        // POST: HistorialEstadoVentas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdHistorialEstadoVenta,IdVenta,IdEstadoVenta,FechaEstado")] HistorialEstadoVenta historialEstadoVenta)
        {
            if (id != historialEstadoVenta.IdHistorialEstadoVenta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(historialEstadoVenta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HistorialEstadoVentaExists(historialEstadoVenta.IdHistorialEstadoVenta))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(historialEstadoVenta);
        }

        // GET: HistorialEstadoVentas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historialEstadoVenta = await _context.HistorialesEstadoVenta
                .FirstOrDefaultAsync(m => m.IdHistorialEstadoVenta == id);
            if (historialEstadoVenta == null)
            {
                return NotFound();
            }

            return View(historialEstadoVenta);
        }

        // POST: HistorialEstadoVentas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var historialEstadoVenta = await _context.HistorialesEstadoVenta.FindAsync(id);
            if (historialEstadoVenta != null)
            {
                _context.HistorialesEstadoVenta.Remove(historialEstadoVenta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HistorialEstadoVentaExists(int id)
        {
            return _context.HistorialesEstadoVenta.Any(e => e.IdHistorialEstadoVenta == id);
        }
    }
}
