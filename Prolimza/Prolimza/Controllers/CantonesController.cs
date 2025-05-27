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
    public class CantonesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CantonesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cantones
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Cantones.Include(c => c.Provincia);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Cantones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var canton = await _context.Cantones
                .Include(c => c.Provincia)
                .FirstOrDefaultAsync(m => m.IdCanton == id);
            if (canton == null)
            {
                return NotFound();
            }

            return View(canton);
        }

        // GET: Cantones/Create
        public IActionResult Create()
        {
            ViewData["IdProvincia"] = new SelectList(_context.Provincias, "IdProvincia", "Nombre");
            return View();
        }

        // POST: Cantones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCanton,Nombre,IdProvincia")] Canton canton)
        {
            if (ModelState.IsValid)
            {
                _context.Add(canton);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdProvincia"] = new SelectList(_context.Provincias, "IdProvincia", "Nombre", canton.IdProvincia);
            return View(canton);
        }

        // GET: Cantones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var canton = await _context.Cantones.FindAsync(id);
            if (canton == null)
            {
                return NotFound();
            }
            ViewData["IdProvincia"] = new SelectList(_context.Provincias, "IdProvincia", "Nombre", canton.IdProvincia);
            return View(canton);
        }

        // POST: Cantones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCanton,Nombre,IdProvincia")] Canton canton)
        {
            if (id != canton.IdCanton)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(canton);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CantonExists(canton.IdCanton))
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
            ViewData["IdProvincia"] = new SelectList(_context.Provincias, "IdProvincia", "Nombre", canton.IdProvincia);
            return View(canton);
        }

        // GET: Cantones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var canton = await _context.Cantones
                .Include(c => c.Provincia)
                .FirstOrDefaultAsync(m => m.IdCanton == id);
            if (canton == null)
            {
                return NotFound();
            }

            return View(canton);
        }

        // POST: Cantones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var canton = await _context.Cantones.FindAsync(id);
            if (canton != null)
            {
                _context.Cantones.Remove(canton);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CantonExists(int id)
        {
            return _context.Cantones.Any(e => e.IdCanton == id);
        }
    }
}
