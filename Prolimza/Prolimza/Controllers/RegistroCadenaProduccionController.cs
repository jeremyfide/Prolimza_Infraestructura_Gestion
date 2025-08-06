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
    public class RegistroCadenaProduccionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegistroCadenaProduccionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RegistroCadenaProduccion
        public async Task<IActionResult> Index()
        {
            return View(await _context.RegistroCadenaProduccion.ToListAsync());
        }

        // GET: RegistroCadenaProduccion/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registroCadenaProduccion = await _context.RegistroCadenaProduccion
                .FirstOrDefaultAsync(m => m.IdRegistro == id);
            if (registroCadenaProduccion == null)
            {
                return NotFound();
            }

            return View(registroCadenaProduccion);
        }

        // GET: RegistroCadenaProduccion/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RegistroCadenaProduccion/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRegistro,Sku,IdProducto,NombreProducto,CantidadFinal,Exito,Mensaje,FechaRegistro,IdUsuario")] RegistroCadenaProduccion registroCadenaProduccion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(registroCadenaProduccion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(registroCadenaProduccion);
        }

        // GET: RegistroCadenaProduccion/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registroCadenaProduccion = await _context.RegistroCadenaProduccion.FindAsync(id);
            if (registroCadenaProduccion == null)
            {
                return NotFound();
            }
            return View(registroCadenaProduccion);
        }

        // POST: RegistroCadenaProduccion/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRegistro,Sku,IdProducto,NombreProducto,CantidadFinal,Exito,Mensaje,FechaRegistro,IdUsuario")] RegistroCadenaProduccion registroCadenaProduccion)
        {
            if (id != registroCadenaProduccion.IdRegistro)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registroCadenaProduccion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistroCadenaProduccionExists(registroCadenaProduccion.IdRegistro))
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
            return View(registroCadenaProduccion);
        }

        // GET: RegistroCadenaProduccion/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registroCadenaProduccion = await _context.RegistroCadenaProduccion
                .FirstOrDefaultAsync(m => m.IdRegistro == id);
            if (registroCadenaProduccion == null)
            {
                return NotFound();
            }

            return View(registroCadenaProduccion);
        }

        // POST: RegistroCadenaProduccion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var registroCadenaProduccion = await _context.RegistroCadenaProduccion.FindAsync(id);
            if (registroCadenaProduccion != null)
            {
                _context.RegistroCadenaProduccion.Remove(registroCadenaProduccion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegistroCadenaProduccionExists(int id)
        {
            return _context.RegistroCadenaProduccion.Any(e => e.IdRegistro == id);
        }
    }
}
