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
    public class MateriaPrimasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MateriaPrimasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MateriaPrimas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.MateriasPrimas.Include(m => m.Bodega);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: MateriaPrimas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materiaPrima = await _context.MateriasPrimas
                .Include(m => m.Bodega)
                .FirstOrDefaultAsync(m => m.IdMateriaPrima == id);
            if (materiaPrima == null)
            {
                return NotFound();
            }

            return View(materiaPrima);
        }

        // GET: MateriaPrimas/Create
        public IActionResult Create()
        {
            ViewData["IdBodega"] = new SelectList(_context.Bodegas, "IdBodega", "IdBodega");
            return View();
        }

        // POST: MateriaPrimas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdMateriaPrima,Nombre,Descripcion,Sku,Cantidad,FechaCaducidad,IdBodega")] MateriaPrima materiaPrima)
        {
            if (ModelState.IsValid)
            {
                _context.Add(materiaPrima);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdBodega"] = new SelectList(_context.Bodegas, "IdBodega", "IdBodega", materiaPrima.IdBodega);
            return View(materiaPrima);
        }

        // GET: MateriaPrimas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materiaPrima = await _context.MateriasPrimas.FindAsync(id);
            if (materiaPrima == null)
            {
                return NotFound();
            }
            ViewData["IdBodega"] = new SelectList(_context.Bodegas, "IdBodega", "IdBodega", materiaPrima.IdBodega);
            return View(materiaPrima);
        }

        // POST: MateriaPrimas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdMateriaPrima,Nombre,Descripcion,Sku,Cantidad,FechaCaducidad,IdBodega")] MateriaPrima materiaPrima)
        {
            if (id != materiaPrima.IdMateriaPrima)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(materiaPrima);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MateriaPrimaExists(materiaPrima.IdMateriaPrima))
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
            ViewData["IdBodega"] = new SelectList(_context.Bodegas, "IdBodega", "IdBodega", materiaPrima.IdBodega);
            return View(materiaPrima);
        }

        // GET: MateriaPrimas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materiaPrima = await _context.MateriasPrimas
                .Include(m => m.Bodega)
                .FirstOrDefaultAsync(m => m.IdMateriaPrima == id);
            if (materiaPrima == null)
            {
                return NotFound();
            }

            return View(materiaPrima);
        }

        // POST: MateriaPrimas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var materiaPrima = await _context.MateriasPrimas.FindAsync(id);
            if (materiaPrima != null)
            {
                _context.MateriasPrimas.Remove(materiaPrima);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MateriaPrimaExists(int id)
        {
            return _context.MateriasPrimas.Any(e => e.IdMateriaPrima == id);
        }
    }
}
