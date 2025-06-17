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
    public class RecetasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecetasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Recetas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Recetas.ToListAsync());
        }

        // GET: Recetas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receta = await _context.Recetas
                .Include(r => r.MateriasReceta)
                    .ThenInclude(mr => mr.MateriaPrima)
                .Include(r => r.Producto)
                .FirstOrDefaultAsync(r => r.IdReceta == id);

            if (receta == null)
            {
                return NotFound();
            }

            receta.MateriasReceta ??= new List<MateriaReceta>(); // ✅ Ensures it's never null

            return PartialView(receta);
        }

        // GET: Recetas/Create
        public IActionResult Create()
        {
            /*
            ViewData["IdProducto"] = new SelectList(_context.Productos, "IdProducto", "NombreProducto");
            return View();
            */
            Receta modelRecetas = new Receta ();
            ViewBag.Productos = new SelectList(_context.Productos, "IdProducto", "NombreProducto");
            ViewBag.MateriasPrimas = new SelectList(_context.MateriasPrimas, "IdMateriaPrima", "Nombre");

            return View(modelRecetas);
        }


        // POST: Recetas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Receta model)
        {
            if (ModelState.IsValid)
            {
                // Ensure the list is not null
                var materias = model.MateriasReceta ?? new List<MateriaReceta>();

                var receta = new Receta
                {
                    Nombre = model.Nombre,
                    Descripcion = model.Descripcion,
                    IdProducto = model.IdProducto,
                    MateriasReceta = materias.Select(m => new MateriaReceta
                    {
                        IdMateriaPrima = m.IdMateriaPrima,
                        Cantidad = m.Cantidad
                    }).ToList()
                };

                _context.Recetas.Add(receta);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Productos = new SelectList(_context.Productos, "IdProducto", "Nombre", model.IdProducto);
            ViewBag.MateriasPrimas = new SelectList(_context.MateriasPrimas, "IdMateriaPrima", "Nombre");

            // Ensure model.MateriasReceta is not null to avoid exception in view
            if (model.MateriasReceta == null)
                model.MateriasReceta = new List<MateriaReceta>();

            return View(model);
        }


        // GET: Recetas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receta = await _context.Recetas
                .Include(r => r.MateriasReceta)
                .FirstOrDefaultAsync(r => r.IdReceta == id);

            if (receta == null)
            {
                return NotFound();
            }

            ViewBag.Productos = new SelectList(_context.Productos, "IdProducto", "NombreProducto", receta.IdProducto);
            ViewBag.MateriasPrimas = new SelectList(_context.MateriasPrimas, "IdMateriaPrima", "Nombre");

            return View(receta);
        }


        // POST: Recetas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Receta receta)
        {
            if (id != receta.IdReceta)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Cargar la receta original
                    var recetaExistente = await _context.Recetas
                        .Include(r => r.MateriasReceta)
                        .FirstOrDefaultAsync(r => r.IdReceta == id);

                    if (recetaExistente == null)
                        return NotFound();

                    // Actualizar campos simples
                    recetaExistente.Nombre = receta.Nombre;
                    recetaExistente.Descripcion = receta.Descripcion;
                    recetaExistente.IdProducto = receta.IdProducto;

                    // Eliminar materias anteriores
                    _context.MateriasReceta.RemoveRange(recetaExistente.MateriasReceta);

                    // Agregar nuevas materias
                    if (receta.MateriasReceta != null)
                    {
                        foreach (var materia in receta.MateriasReceta)
                        {
                            recetaExistente.MateriasReceta.Add(new MateriaReceta
                            {
                                IdMateriaPrima = materia.IdMateriaPrima,
                                Cantidad = materia.Cantidad,
                                IdReceta = recetaExistente.IdReceta
                            });
                        }
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecetaExists(receta.IdReceta))
                        return NotFound();
                    else
                        throw;
                }
            }

            ViewBag.Productos = new SelectList(_context.Productos, "IdProducto", "NombreProducto", receta.IdProducto);
            ViewBag.MateriasPrimas = new SelectList(_context.MateriasPrimas, "IdMateriaPrima", "Nombre");
            return View(receta);
        }



        // GET: Recetas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receta = await _context.Recetas
                .FirstOrDefaultAsync(m => m.IdReceta == id);
            if (receta == null)
            {
                return NotFound();
            }

            return View(receta);
        }

        // POST: Recetas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var receta = await _context.Recetas.FindAsync(id);
            if (receta != null)
            {
                _context.Recetas.Remove(receta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecetaExists(int id)
        {
            return _context.Recetas.Any(e => e.IdReceta == id);
        }
    }
}
