using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Data;
using Prolimza.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prolimza.Services.IA
{
    public class PrediccionService
    {
        private readonly ApplicationDbContext _context;
        private readonly MLContext _mlContext;

        public PrediccionService(ApplicationDbContext context)
        {
            _context = context;
            _mlContext = new MLContext();
        }

        public List<(int Mes, int Anno, float Prediccion)> PredecirVentas(
            int idProducto,
            int mesesFuturos = 6)
        {
            // 1. Agrupar ventas por mes/año directamente en la BD
            var historicoMensual = _context.DetallesVenta
                .Where(d => d.IdProducto == idProducto)
                .GroupBy(d => new { d.Venta.FechaVenta.Year, d.Venta.FechaVenta.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Cantidad = g.Sum(x => x.Cantidad)
                })
                .OrderBy(x => x.Year * 12 + x.Month)
                .AsNoTracking()
                .ToList();

            var resultados = new List<(int Mes, int Anno, float Prediccion)>();

            if (!historicoMensual.Any())
                return resultados;

            // 2. (Opcional) Limitar el número de meses históricos
            const int MAX_MESES_HIST = 36; // por ejemplo, últimos 3 años
            if (historicoMensual.Count > MAX_MESES_HIST)
            {
                historicoMensual = historicoMensual
                    .Skip(historicoMensual.Count - MAX_MESES_HIST)
                    .ToList();
            }

            // 3. Mapear a la clase de entrada del modelo
            var datosEntrenamiento = historicoMensual
                .Select((x, index) => new PrediccionEntrada
                {
                    Secuencia = index + 1,          // 1,2,3,...n
                    IdProducto = idProducto,        // lo dejamos por compatibilidad, pero no lo usamos como feature
                    CantidadHistorica = x.Cantidad
                })
                .ToList();

            var trainData = _mlContext.Data.LoadFromEnumerable(datosEntrenamiento);

            var pipeline = _mlContext.Transforms.CopyColumns(
                                outputColumnName: "Label",
                                inputColumnName: nameof(PrediccionEntrada.CantidadHistorica))
                .Append(_mlContext.Transforms.Concatenate(
                                "Features",
                                nameof(PrediccionEntrada.Secuencia)))
                .Append(_mlContext.Regression.Trainers.Sdca());

            var model = pipeline.Fit(trainData);

            // 6. Preparar entradas futuras en batch
            int totalHistorico = datosEntrenamiento.Count;
            var entradasFuturas = new List<PrediccionEntrada>();

            for (int i = 1; i <= mesesFuturos; i++)
            {
                entradasFuturas.Add(new PrediccionEntrada
                {
                    Secuencia = totalHistorico + i,
                    IdProducto = idProducto,
                    CantidadHistorica = 0
                });
            }

            var futurosData = _mlContext.Data.LoadFromEnumerable(entradasFuturas);
            var predDataView = model.Transform(futurosData);

            var predicciones = _mlContext.Data
                .CreateEnumerable<PrediccionResultado>(predDataView, reuseRowObject: false)
                .ToList();

            // 8. Armar el retorno con (Mes, Anno, Prediccion)
            var hoy = DateTime.Today;
            for (int i = 0; i < mesesFuturos; i++)
            {
                var fecha = hoy.AddMonths(i + 1); // igual que tu lógica original
                var pred = predicciones[i].Prediccion;

                // opcional: evitar negativos
                if (pred < 0) pred = 0;

                resultados.Add((fecha.Month, fecha.Year, pred));
            }

            return resultados;
        }

    }
}
