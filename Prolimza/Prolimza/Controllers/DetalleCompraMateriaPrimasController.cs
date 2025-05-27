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
    public class DetalleCompraMateriaPrimasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DetalleCompraMateriaPrimasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DetalleCompraMateriaPrimas
        public async Task<IActionResult> Index()
        {
            return View(await _context.DetallesCompraMateriaPrimas.ToListAsync());
        }

        // GET: DetalleCompraMateriaPrimas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleCompraMateriaPrima = await _context.DetallesCompraMateriaPrimas
                .FirstOrDefaultAsync(m => m.IdCompra == id);
            if (detalleCompraMateriaPrima == null)
            {
                return NotFound();
            }

            return View(detalleCompraMateriaPrima);
        }

        // GET: DetalleCompraMateriaPrimas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DetalleCompraMateriaPrimas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCompra,IdMateriaPrima,Cantidad")] DetalleCompraMateriaPrima detalleCompraMateriaPrima)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detalleCompraMateriaPrima);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(detalleCompraMateriaPrima);
        }

        // GET: DetalleCompraMateriaPrimas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleCompraMateriaPrima = await _context.DetallesCompraMateriaPrimas.FindAsync(id);
            if (detalleCompraMateriaPrima == null)
            {
                return NotFound();
            }
            return View(detalleCompraMateriaPrima);
        }

        // POST: DetalleCompraMateriaPrimas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCompra,IdMateriaPrima,Cantidad")] DetalleCompraMateriaPrima detalleCompraMateriaPrima)
        {
            if (id != detalleCompraMateriaPrima.IdCompra)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detalleCompraMateriaPrima);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetalleCompraMateriaPrimaExists(detalleCompraMateriaPrima.IdCompra))
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
            return View(detalleCompraMateriaPrima);
        }

        // GET: DetalleCompraMateriaPrimas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleCompraMateriaPrima = await _context.DetallesCompraMateriaPrimas
                .FirstOrDefaultAsync(m => m.IdCompra == id);
            if (detalleCompraMateriaPrima == null)
            {
                return NotFound();
            }

            return View(detalleCompraMateriaPrima);
        }

        // POST: DetalleCompraMateriaPrimas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var detalleCompraMateriaPrima = await _context.DetallesCompraMateriaPrimas.FindAsync(id);
            if (detalleCompraMateriaPrima != null)
            {
                _context.DetallesCompraMateriaPrimas.Remove(detalleCompraMateriaPrima);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DetalleCompraMateriaPrimaExists(int id)
        {
            return _context.DetallesCompraMateriaPrimas.Any(e => e.IdCompra == id);
        }
    }
}
