using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Prolimza.Models;

namespace Prolimza.Controllers
{
    [Authorize(Roles = "Administrador,Operador")]
    public class ComprasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComprasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Compras
        public async Task<IActionResult> Index()
        {
            var compras = await _context.Compras
                .Include(c => c.Usuario)
                .ToListAsync();

            return View(compras);
        }

        // GET: Compras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var compra = await _context.Compras
                .Include(c => c.Usuario)
                .Include(c => c.DetalleCompraProductos)
                    .ThenInclude(dc => dc.Producto)
                .Include(c => c.DetalleCompraMateriaPrimas)
                    .ThenInclude(mpc => mpc.MateriaPrima)
                .FirstOrDefaultAsync(m => m.IdCompra == id);

            if (compra == null)
                return NotFound();

            compra.DetalleCompraProductos ??= new List<DetalleCompraProducto>();
            compra.DetalleCompraMateriaPrimas ??= new List<DetalleCompraMateriaPrima>();

            return PartialView(compra);
        }

        // GET: Compras/Create
        public IActionResult Create()
        {
            var viewModel = new CompraCreateViewModel
            {
                ListaProductos = _context.Productos
                    .Select(p => new SelectListItem { Value = p.IdProducto.ToString(), Text = p.NombreProducto })
                    .ToList(),
                ListaMateriasPrimas = _context.MateriasPrimas
                    .Select(mp => new SelectListItem { Value = mp.IdMateriaPrima.ToString(), Text = mp.Nombre })
                    .ToList()
            };

            return View(viewModel);
        }

        // POST: Compras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(CompraCreateViewModel viewModel)
        {
            if (
                (viewModel.Productos == null || !viewModel.Productos.Any(p => p.Cantidad > 0)) &&
                (viewModel.MateriasPrimas == null || !viewModel.MateriasPrimas.Any(mp => mp.Cantidad > 0))
            )
            {
                ModelState.AddModelError("", "Debe agregar al menos un producto o una materia prima con cantidad mayor a 0.");
            }


            var userIdString = User.FindFirst("IdUsuario")?.Value;
            if (int.TryParse(userIdString, out int idUsuario))
            {
                viewModel.Compra.IdUsuario = idUsuario;
            }


            if (ModelState.IsValid)
            {
                _context.Compras.Add(viewModel.Compra);
                await _context.SaveChangesAsync();

                // Agregar productos comprados y actualizar stock
                foreach (var prod in viewModel.Productos.Where(p => p.Cantidad > 0))
                {
                    prod.IdCompra = viewModel.Compra.IdCompra;
                    _context.DetallesCompraProductos.Add(prod);

                    var productoEnBD = await _context.Productos.FindAsync(prod.IdProducto);
                    if (productoEnBD != null)
                    {
                        productoEnBD.Cantidad += prod.Cantidad;
                        _context.Productos.Update(productoEnBD);
                    }
                }

                // Agregar materias primas compradas y actualizar stock
                foreach (var mp in viewModel.MateriasPrimas.Where(m => m.Cantidad > 0))
                {
                    mp.IdCompra = viewModel.Compra.IdCompra;
                    _context.DetallesCompraMateriaPrimas.Add(mp);

                    var materiaEnBD = await _context.MateriasPrimas.FindAsync(mp.IdMateriaPrima);
                    if (materiaEnBD != null)
                    {
                        materiaEnBD.Cantidad += mp.Cantidad;
                        _context.MateriasPrimas.Update(materiaEnBD);
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }


            // Recargar listas si hubo error
            viewModel.ListaProductos = _context.Productos
                .Select(p => new SelectListItem { Value = p.IdProducto.ToString(), Text = p.NombreProducto })
                .ToList();

            viewModel.ListaMateriasPrimas = _context.MateriasPrimas
                .Select(mp => new SelectListItem { Value = mp.IdMateriaPrima.ToString(), Text = mp.Nombre })
                .ToList();

            return View(viewModel);
        }



        // GET: Compras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compras.FindAsync(id);
            if (compra == null)
            {
                return NotFound();
            }
            var usuarios = await _context.Usuarios
                .Select(u => new { u.IdUsuario, u.Nombre })
                .ToListAsync();

            ViewData["Usuarios"] = new SelectList(usuarios, "IdUsuario", "Nombre", compra.IdUsuario);
    
            return View(compra);
        }

        // POST: Compras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCompra,FechaCompra,IdUsuario,CodigoIngreso")] Compra compra)
        {
            if (id != compra.IdCompra)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(compra);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompraExists(compra.IdCompra))
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
            return View(compra);
        }

        // GET: Compras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compras
                .FirstOrDefaultAsync(m => m.IdCompra == id);
            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }

        // POST: Compras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var compra = await _context.Compras.FindAsync(id);
            if (compra != null)
            {
                _context.Compras.Remove(compra);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompraExists(int id)
        {
            return _context.Compras.Any(e => e.IdCompra == id);
        }
    }
}
