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
    public class DetalleVentasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DetalleVentasController(ApplicationDbContext context)
        {
            _context = context;
        }



        // GET: DetalleVentas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleVenta = await _context.DetallesVenta
                .FirstOrDefaultAsync(m => m.IdVenta == id);
            if (detalleVenta == null)
            {
                return NotFound();
            }

            return View(detalleVenta);
        }

        // GET: DetalleVentas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DetalleVentas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdVenta,IdProducto,Cantidad")] DetalleVenta detalleVenta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detalleVenta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(detalleVenta);
        }

        // GET: DetalleVentas/Edit/5/10
        public async Task<IActionResult> Edit(int? idVenta, int? idProducto)
        {
            if (idVenta == null || idProducto == null)
            {
                return NotFound();
            }

            var detalleVenta = await _context.DetallesVenta
                .FirstOrDefaultAsync(m => m.IdVenta == idVenta && m.IdProducto == idProducto);
            if (detalleVenta == null)
            {
                return NotFound();
            }

            return View(detalleVenta);
        }

        // POST: DetalleVentas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdVenta,IdProducto,Cantidad")] DetalleVenta detalleVenta)
        {
            if (id != detalleVenta.IdVenta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detalleVenta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetalleVentaExists(detalleVenta.IdVenta))
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
            return View(detalleVenta);
        }

        // GET: DetalleVentas/Delete/5
        public async Task<IActionResult> Delete(int? idVenta, int? idProducto)
        {
            if (idVenta == null || idProducto == null)
            {
                return NotFound();
            }

            var detalleVenta = await _context.DetallesVenta
                .FirstOrDefaultAsync(m => m.IdVenta == idVenta && m.IdProducto == idProducto);
            if (detalleVenta == null)
            {
                return NotFound();
            }

            return View(detalleVenta);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int idVenta, int idProducto)
        {
            var detalleVenta = await _context.DetallesVenta
                .FirstOrDefaultAsync(m => m.IdVenta == idVenta && m.IdProducto == idProducto);

            if (detalleVenta != null)
            {
                _context.DetallesVenta.Remove(detalleVenta);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: DetalleVenta/CreateMultiple
        public IActionResult CreateMultiple(int idVenta)
        {
            var productos = _context.Productos.ToList();
            ViewBag.IdVenta = idVenta;
            return View(productos);
        }

        // POST: DetalleVenta/CreateMultiple
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMultiple(int idVenta, List<DetalleVenta> detalles)
        {
            foreach (var detalle in detalles)
            {
                if (detalle.Cantidad > 0)
                {
                    detalle.IdVenta = idVenta;
                    _context.DetallesVenta.Add(detalle);
                }
            }

            await _context.SaveChangesAsync();
            return View();
        }


        // GET: Ventas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Ventas.ToListAsync());
        }

        public async Task<IActionResult> ListaAsync()
        {
            return View(await _context.DetallesVenta.ToListAsync());
        }
        private bool DetalleVentaExists(int id)
        {
            return _context.DetallesVenta.Any(e => e.IdVenta == id);
        }
    }
}
