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
    public class DetalleCompraProductosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DetalleCompraProductosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DetalleCompraProductos
        public async Task<IActionResult> Index()
        {
            return View(await _context.DetallesCompraProductos.ToListAsync());
        }

        // GET: DetalleCompraProductos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleCompraProducto = await _context.DetallesCompraProductos
                .FirstOrDefaultAsync(m => m.IdCompra == id);
            if (detalleCompraProducto == null)
            {
                return NotFound();
            }

            return View(detalleCompraProducto);
        }

        // GET: DetalleCompraProductos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DetalleCompraProductos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCompra,IdProducto,Cantidad")] DetalleCompraProducto detalleCompraProducto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detalleCompraProducto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(detalleCompraProducto);
        }

        // GET: DetalleCompraProductos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleCompraProducto = await _context.DetallesCompraProductos.FindAsync(id);
            if (detalleCompraProducto == null)
            {
                return NotFound();
            }
            return View(detalleCompraProducto);
        }

        // POST: DetalleCompraProductos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCompra,IdProducto,Cantidad")] DetalleCompraProducto detalleCompraProducto)
        {
            if (id != detalleCompraProducto.IdCompra)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detalleCompraProducto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetalleCompraProductoExists(detalleCompraProducto.IdCompra))
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
            return View(detalleCompraProducto);
        }

        // GET: DetalleCompraProductos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleCompraProducto = await _context.DetallesCompraProductos
                .FirstOrDefaultAsync(m => m.IdCompra == id);
            if (detalleCompraProducto == null)
            {
                return NotFound();
            }

            return View(detalleCompraProducto);
        }

        // POST: DetalleCompraProductos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var detalleCompraProducto = await _context.DetallesCompraProductos.FindAsync(id);
            if (detalleCompraProducto != null)
            {
                _context.DetallesCompraProductos.Remove(detalleCompraProducto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DetalleCompraProductoExists(int id)
        {
            return _context.DetallesCompraProductos.Any(e => e.IdCompra == id);
        }
    }
}
