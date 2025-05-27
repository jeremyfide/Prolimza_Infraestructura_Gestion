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
    public class MateriaRecetasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MateriaRecetasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MateriaRecetas
        public async Task<IActionResult> Index()
        {
            return View(await _context.MateriasReceta.ToListAsync());
        }

        // GET: MateriaRecetas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materiaReceta = await _context.MateriasReceta
                .FirstOrDefaultAsync(m => m.IdReceta == id);
            if (materiaReceta == null)
            {
                return NotFound();
            }

            return View(materiaReceta);
        }

        // GET: MateriaRecetas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MateriaRecetas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdReceta,IdMateriaPrima,Cantidad")] MateriaReceta materiaReceta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(materiaReceta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(materiaReceta);
        }

        // GET: MateriaRecetas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materiaReceta = await _context.MateriasReceta.FindAsync(id);
            if (materiaReceta == null)
            {
                return NotFound();
            }
            return View(materiaReceta);
        }

        // POST: MateriaRecetas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdReceta,IdMateriaPrima,Cantidad")] MateriaReceta materiaReceta)
        {
            if (id != materiaReceta.IdReceta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(materiaReceta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MateriaRecetaExists(materiaReceta.IdReceta))
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
            return View(materiaReceta);
        }

        // GET: MateriaRecetas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materiaReceta = await _context.MateriasReceta
                .FirstOrDefaultAsync(m => m.IdReceta == id);
            if (materiaReceta == null)
            {
                return NotFound();
            }

            return View(materiaReceta);
        }

        // POST: MateriaRecetas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var materiaReceta = await _context.MateriasReceta.FindAsync(id);
            if (materiaReceta != null)
            {
                _context.MateriasReceta.Remove(materiaReceta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MateriaRecetaExists(int id)
        {
            return _context.MateriasReceta.Any(e => e.IdReceta == id);
        }
    }
}
