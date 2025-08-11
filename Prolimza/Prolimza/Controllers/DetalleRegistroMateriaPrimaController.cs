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
    public class DetalleRegistroMateriaPrimaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DetalleRegistroMateriaPrimaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DetalleRegistroMateriaPrima
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DetalleRegistroMateriaPrima.Include(d => d.Registro);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DetalleRegistroMateriaPrima/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleRegistroMateriaPrima = await _context.DetalleRegistroMateriaPrima
                .Include(d => d.Registro)
                .FirstOrDefaultAsync(m => m.IdRegistro == id);
            if (detalleRegistroMateriaPrima == null)
            {
                return NotFound();
            }

            return View(detalleRegistroMateriaPrima);
        }

        // GET: DetalleRegistroMateriaPrima/Create
        public IActionResult Create()
        {
            ViewData["IdRegistro"] = new SelectList(_context.RegistroCadenaProduccion, "IdRegistro", "IdRegistro");
            return View();
        }

        // POST: DetalleRegistroMateriaPrima/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDetalle,IdRegistro,IdMateriaPrima,NombreMateriaPrima,CantidadConsumida")] DetalleRegistroMateriaPrima detalleRegistroMateriaPrima)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detalleRegistroMateriaPrima);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdRegistro"] = new SelectList(_context.RegistroCadenaProduccion, "IdRegistro", "IdRegistro", detalleRegistroMateriaPrima.IdRegistro);
            return View(detalleRegistroMateriaPrima);
        }

        // GET: DetalleRegistroMateriaPrima/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleRegistroMateriaPrima = await _context.DetalleRegistroMateriaPrima.FindAsync(id);
            if (detalleRegistroMateriaPrima == null)
            {
                return NotFound();
            }
            ViewData["IdRegistro"] = new SelectList(_context.RegistroCadenaProduccion, "IdRegistro", "IdRegistro", detalleRegistroMateriaPrima.IdRegistro);
            return View(detalleRegistroMateriaPrima);
        }

        // POST: DetalleRegistroMateriaPrima/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDetalle,IdRegistro,IdMateriaPrima,NombreMateriaPrima,CantidadConsumida")] DetalleRegistroMateriaPrima detalleRegistroMateriaPrima)
        {
            if (id != detalleRegistroMateriaPrima.IdRegistro)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detalleRegistroMateriaPrima);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetalleRegistroMateriaPrimaExists(detalleRegistroMateriaPrima.IdRegistro))
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
            ViewData["IdRegistro"] = new SelectList(_context.RegistroCadenaProduccion, "IdRegistro", "IdRegistro", detalleRegistroMateriaPrima.IdRegistro);
            return View(detalleRegistroMateriaPrima);
        }

        // GET: DetalleRegistroMateriaPrima/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleRegistroMateriaPrima = await _context.DetalleRegistroMateriaPrima
                .Include(d => d.Registro)
                .FirstOrDefaultAsync(m => m.IdRegistro == id);
            if (detalleRegistroMateriaPrima == null)
            {
                return NotFound();
            }

            return View(detalleRegistroMateriaPrima);
        }

        // POST: DetalleRegistroMateriaPrima/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var detalleRegistroMateriaPrima = await _context.DetalleRegistroMateriaPrima.FindAsync(id);
            if (detalleRegistroMateriaPrima != null)
            {
                _context.DetalleRegistroMateriaPrima.Remove(detalleRegistroMateriaPrima);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DetalleRegistroMateriaPrimaExists(int id)
        {
            return _context.DetalleRegistroMateriaPrima.Any(e => e.IdRegistro == id);
        }
    }
}
