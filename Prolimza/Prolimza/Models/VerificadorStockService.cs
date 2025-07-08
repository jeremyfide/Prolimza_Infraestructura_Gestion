using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Prolimza.Models
{
    public class VerificadorStockService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeSpan intervalo = TimeSpan.FromMinutes(1);

        public VerificadorStockService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await VerificarStockAsync();
                await Task.Delay(intervalo, stoppingToken);
            }
        }

        private async Task VerificarStockAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            //Verificación de Ventas
            var ventasConEstados = await dbContext.Ventas
                .Include(v => v.HistorialesEstadoVenta)
                    .ThenInclude(h => h.EstadoVenta)
                .ToListAsync();


            foreach (var venta in ventasConEstados)
            {
                var ultimoEstado = venta.HistorialesEstadoVenta
                    .OrderByDescending(h => h.FechaEstado)
                    .FirstOrDefault();

                if (ultimoEstado?.EstadoVenta?.Descripcion == "Pendiente")
                    {
                    var diasEnPendiente = (DateTime.Now - ultimoEstado.FechaEstado).TotalDays;

                    if (diasEnPendiente > 8)
                    {
                        string mensaje = $"La venta con código {venta.CodigoIngreso} está en estado 'Pendiente' desde hace más de 8 días.";

                        bool yaExiste = await dbContext.Alertas
                            .AnyAsync(a => a.Tipo == "Venta" && a.Estado == "Pendiente" && a.Descripcion == mensaje);

                        if (!yaExiste)
                        {
                            dbContext.Alertas.Add(new Alerta
                            {
                                Tipo = "Venta",
                                Descripcion = mensaje,
                                FechaAlerta = DateTime.Now,
                                Estado = "Pendiente"
                            });
                        }
                    }

                }
                else
                {
                    // La venta ya no está pendiente, pero puede tener una alerta activa
                    string mensajeParcial = $"La venta con código {venta.CodigoIngreso} está en estado 'Pendiente'";
                    var alertasPendientes = await dbContext.Alertas
                        .Where(a => a.Tipo == "Venta" && a.Estado == "Pendiente" && a.Descripcion.Contains(mensajeParcial))
                        .ToListAsync();

                    foreach (var alerta in alertasPendientes)
                    {
                        alerta.Estado = "Resuelta";
                    }
                }
            }

            // Verificación de Productos
            var productos = await dbContext.Productos.ToListAsync();
            foreach (var p in productos)
            {
                string mensaje = $"Producto '{p.NombreProducto}' está por debajo del mínimo con {p.Cantidad} unidades.";

                if (p.Cantidad <= p.stockMinimo)
                {
                    // Crear alerta si no existe una pendiente
                    bool yaExiste = await dbContext.Alertas
                        .AnyAsync(a => a.Tipo == "Stock" && a.Estado == "Pendiente" && a.Descripcion == mensaje);

                    if (!yaExiste)
                    {
                        dbContext.Alertas.Add(new Alerta
                        {
                            Tipo = "Stock",
                            Descripcion = mensaje,
                            FechaAlerta = DateTime.Now,
                            Estado = "Pendiente"
                        });
                    }
                }
                else
                {
                    // Marcar como resuelta si ya no está por debajo del mínimo
                    var alertasPendientes = await dbContext.Alertas
                        .Where(a => a.Tipo == "Stock" && a.Estado == "Pendiente" &&
                                    a.Descripcion.Contains($"Producto '{p.NombreProducto}'"))
                        .ToListAsync();

                    foreach (var alerta in alertasPendientes)
                    {
                        alerta.Estado = "Resuelta";
                    }
                }
            }

            // Verificación de Materias Primas
            var materias = await dbContext.MateriasPrimas.ToListAsync();
            foreach (var m in materias)
            {
                string mensaje = $"Materia prima '{m.Nombre}' está por debajo del mínimo con {m.Cantidad} unidades.";

                if (m.Cantidad <= m.stockMinimo)
                {
                    bool yaExiste = await dbContext.Alertas
                        .AnyAsync(a => a.Tipo == "Stock" && a.Estado == "Pendiente" && a.Descripcion == mensaje);

                    if (!yaExiste)
                    {
                        dbContext.Alertas.Add(new Alerta
                        {
                            Tipo = "Stock",
                            Descripcion = mensaje,
                            FechaAlerta = DateTime.Now,
                            Estado = "Pendiente"
                        });
                    }
                }
                else
                {
                    var alertasPendientes = await dbContext.Alertas
                        .Where(a => a.Tipo == "Stock" && a.Estado == "Pendiente" &&
                                    a.Descripcion.Contains($"Materia prima '{m.Nombre}'"))
                        .ToListAsync();

                    foreach (var alerta in alertasPendientes)
                    {
                        alerta.Estado = "Resuelta";
                    }
                }
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
