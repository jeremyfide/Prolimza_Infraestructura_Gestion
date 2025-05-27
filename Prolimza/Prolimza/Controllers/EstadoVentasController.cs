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
    public class EstadoVentasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EstadoVentasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EstadoVentas
        public async Task<IActionResult> Index()
        {
            return View(await _context.EstadosVenta.ToListAsync());
        }

        // GET: EstadoVentas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadoVenta = await _context.EstadosVenta
                .FirstOrDefaultAsync(m => m.IdEstadoVenta == id);
            if (estadoVenta == null)
            {
                return NotFound();
            }

            return View(estadoVenta);
        }

        // GET: EstadoVentas/Create
        public IActionResult Create()
        {
            ViewData["IdEstadoVenta"] = new SelectList(_context.EstadosVenta, "IdEstadoVenta", "Descripcion");
            return View();
        }

        // POST: EstadoVentas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEstadoVenta,Descripcion")] EstadoVenta estadoVenta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estadoVenta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEstadoVenta"] = new SelectList(_context.EstadosVenta, "IdEstadoVenta", "Descripcion");
            return View(estadoVenta);
        }

        // GET: EstadoVentas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadoVenta = await _context.EstadosVenta.FindAsync(id);
            if (estadoVenta == null)
            {
                return NotFound();
            }
            ViewData["IdEstadoVenta"] = new SelectList(_context.EstadosVenta, "IdEstadoVenta", "Descripcion");
            return View(estadoVenta);
        }

        // POST: EstadoVentas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEstadoVenta,Descripcion")] EstadoVenta estadoVenta)
        {
            if (id != estadoVenta.IdEstadoVenta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estadoVenta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstadoVentaExists(estadoVenta.IdEstadoVenta))
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
            ViewData["IdEstadoVenta"] = new SelectList(_context.EstadosVenta, "IdEstadoVenta", "Descripcion");
            return View(estadoVenta);
        }

        // GET: EstadoVentas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadoVenta = await _context.EstadosVenta
                .FirstOrDefaultAsync(m => m.IdEstadoVenta == id);
            if (estadoVenta == null)
            {
                return NotFound();
            }

            return View(estadoVenta);
        }

        // POST: EstadoVentas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estadoVenta = await _context.EstadosVenta.FindAsync(id);
            if (estadoVenta != null)
            {
                _context.EstadosVenta.Remove(estadoVenta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstadoVentaExists(int id)
        {
            return _context.EstadosVenta.Any(e => e.IdEstadoVenta == id);
        }
    }
}
