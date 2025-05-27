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
    public class DistritosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DistritosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Distritos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Distritos.Include(d => d.Canton);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Distritos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distrito = await _context.Distritos
                .Include(d => d.Canton)
                .FirstOrDefaultAsync(m => m.IdDistrito == id);
            if (distrito == null)
            {
                return NotFound();
            }

            return View(distrito);
        }

        // GET: Distritos/Create
        public IActionResult Create()
        {
            ViewData["IdCanton"] = new SelectList(_context.Cantones, "IdCanton", "Nombre");
            return View();
        }

        // POST: Distritos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDistrito,Nombre,IdCanton")] Distrito distrito)
        {
            if (ModelState.IsValid)
            {
                _context.Add(distrito);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCanton"] = new SelectList(_context.Cantones, "IdCanton", "Nombre", distrito.IdCanton);
            return View(distrito);
        }

        // GET: Distritos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distrito = await _context.Distritos.FindAsync(id);
            if (distrito == null)
            {
                return NotFound();
            }
            ViewData["IdCanton"] = new SelectList(_context.Cantones, "IdCanton", "Nombre", distrito.IdCanton);
            return View(distrito);
        }

        // POST: Distritos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDistrito,Nombre,IdCanton")] Distrito distrito)
        {
            if (id != distrito.IdDistrito)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(distrito);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DistritoExists(distrito.IdDistrito))
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
            ViewData["IdCanton"] = new SelectList(_context.Cantones, "IdCanton", "Nombre", distrito.IdCanton);
            return View(distrito);
        }

        // GET: Distritos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var distrito = await _context.Distritos
                .Include(d => d.Canton)
                .FirstOrDefaultAsync(m => m.IdDistrito == id);
            if (distrito == null)
            {
                return NotFound();
            }

            return View(distrito);
        }

        // POST: Distritos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var distrito = await _context.Distritos.FindAsync(id);
            if (distrito != null)
            {
                _context.Distritos.Remove(distrito);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DistritoExists(int id)
        {
            return _context.Distritos.Any(e => e.IdDistrito == id);
        }
    }
}
