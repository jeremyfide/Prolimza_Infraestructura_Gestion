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

        public List<(int Mes, int Anno, float Prediccion)> PredecirVentas(int idProducto, int mesesFuturos = 6)
        {
            var datos = _context.DetallesVenta
                .Where(d => d.IdProducto == idProducto)
                .Select(d => new { d.Cantidad, d.Venta.FechaVenta })
                .ToList();

            var agrupado = datos
                .GroupBy(d => new { d.FechaVenta.Year, d.FechaVenta.Month })
                .OrderBy(g => g.Key.Year * 12 + g.Key.Month)
                .Select((g, index) => new PrediccionEntrada
                {
                    Secuencia = index + 1,
                    IdProducto = idProducto,
                    CantidadHistorica = g.Sum(x => x.Cantidad)
                })
                .ToList();

            foreach (var r in agrupado)
            {
                Console.WriteLine($"Secuencia: {r.Secuencia}, Producto: {r.IdProducto}, Cantidad: {r.CantidadHistorica}");
            }


            var trainData = _mlContext.Data.LoadFromEnumerable(agrupado);

            var pipeline = _mlContext.Transforms.CopyColumns("Label", nameof(PrediccionEntrada.CantidadHistorica))
                .Append(_mlContext.Transforms.Concatenate("Features", nameof(PrediccionEntrada.Secuencia), nameof(PrediccionEntrada.IdProducto)))
                .Append(_mlContext.Regression.Trainers.Sdca());


            var model = pipeline.Fit(trainData);
            var predEngine = _mlContext.Model.CreatePredictionEngine<PrediccionEntrada, PrediccionResultado>(model);

            var resultados = new List<(int Mes, int Anno, float Prediccion)>();
            var hoy = DateTime.Today;

            for (int i = 1; i <= mesesFuturos; i++)
            {
                var entrada = new PrediccionEntrada
                {
                    Secuencia = agrupado.Count + i,
                    IdProducto = idProducto,
                    CantidadHistorica = 0
                };

                var resultado = predEngine.Predict(entrada);
                resultados.Add((hoy.AddMonths(i).Month, hoy.AddMonths(i).Year, resultado.Prediccion));
            }

            var predictions = model.Transform(trainData);
            var metrics = _mlContext.Regression.Evaluate(predictions);
            Console.WriteLine($"R²: {metrics.RSquared}, RMSE: {metrics.RootMeanSquaredError}");

            return resultados;
        }
    }
}
