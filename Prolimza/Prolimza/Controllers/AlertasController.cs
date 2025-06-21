using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Prolimza.Models;

namespace Prolimza.Controllers
{
    public class AlertasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AlertasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Listado completo de alertas
        public async Task<IActionResult> Index()
        {
            var alertas = await _context.Alertas.Include(a => a.Usuario).ToListAsync();
            return View(alertas);
        }

        // Detalle de una alerta
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var alerta = await _context.Alertas
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(m => m.IdAlerta == id);

            if (alerta == null) return NotFound();

            return View(alerta);
        }

        // Crear nueva alerta
        public IActionResult Create()
        {
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAlerta,Tipo,Descripcion,FechaAlerta,Estado,IdUsuario")] Alerta alerta)
        {
            if (!ModelState.IsValid) return View(alerta);

            _context.Add(alerta);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Editar alerta
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var alerta = await _context.Alertas.FindAsync(id);
            if (alerta == null) return NotFound();

            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", alerta.IdUsuario);
            return View(alerta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAlerta,Tipo,Descripcion,FechaAlerta,Estado,IdUsuario")] Alerta alerta)
        {
            if (id != alerta.IdAlerta) return NotFound();

            if (!ModelState.IsValid) return View(alerta);

            try
            {
                _context.Update(alerta);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlertaExists(alerta.IdAlerta)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // Eliminar alerta
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var alerta = await _context.Alertas
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(m => m.IdAlerta == id);

            if (alerta == null) return NotFound();

            return View(alerta);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var alerta = await _context.Alertas.FindAsync(id);
            if (alerta != null) _context.Alertas.Remove(alerta);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool AlertaExists(int id) => _context.Alertas.Any(e => e.IdAlerta == id);

        // GET: Alertas/GetPendientes - Solo pendientes
        public async Task<IActionResult> GetPendientes()
        {
            var pendientes = await _context.Alertas
                .Where(a => a.Estado == "Pendiente")
                .OrderByDescending(a => a.FechaAlerta)
                .Select(a => new {
                    a.IdAlerta,
                    a.Descripcion,
                    FechaAlerta = a.FechaAlerta.ToString("g")
                })
                .ToListAsync();

            return Json(pendientes);
        }

        // POST: Alertas/Acknowledge - Marcar como revisada
        [HttpPost]
        public async Task<IActionResult> Acknowledge([FromBody] AcknowledgeRequest request)
        {
            var alerta = await _context.Alertas.FindAsync(request.Id);
            if (alerta == null) return NotFound();

            alerta.Estado = "Revisada";
            await _context.SaveChangesAsync();

            return Ok();
        }

        public class AcknowledgeRequest
        {
            public int Id { get; set; }
        }
    }
}
