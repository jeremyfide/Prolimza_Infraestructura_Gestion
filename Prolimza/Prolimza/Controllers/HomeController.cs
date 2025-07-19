using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Prolimza.Models;

namespace Prolimza.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult ForzarError()
        {
            throw new Exception("Este es un error de prueba generado manualmente.");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            var errorMessage = exceptionFeature?.Error.Message;
            var stackTrace = exceptionFeature?.Error.StackTrace;
            var log = $"Error: {errorMessage}, RequestID: {requestId}, StackTrace: {stackTrace}";

            string resumen = $"Error: {errorMessage}, RequestID: {requestId}";
            string stackResumen = stackTrace != null && stackTrace.Length > 100
                ? stackTrace.Substring(0, 100) + "..."
                : stackTrace ?? "";

            string logFinal = resumen + ", Stack: " + stackResumen;

            if (logFinal.Length > 255)
            {
                logFinal = logFinal.Substring(0, 252) + "...";
            }

            var auditoria = new Auditoria
            {
                Log = logFinal,
                TipoEvento = "Crítico",
                FechaEvento = DateTime.Now,
                IdUsuario = 1
            };

            _context.Auditorias.Add(auditoria);
            await _context.SaveChangesAsync();

            _logger.LogError(exceptionFeature?.Error, "Excepción no controlada");

            return View(new ErrorViewModel { RequestId = requestId });
        }
    }
}
