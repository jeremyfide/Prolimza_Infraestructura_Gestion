using Microsoft.ML.Data;
using System;

namespace Prolimza.Models
{
    public class PrediccionEntrada
    {
        [LoadColumn(0)]
        public float Secuencia { get; set; }

        [LoadColumn(1)]
        public float IdProducto { get; set; }

        [LoadColumn(2)]
        public float CantidadHistorica { get; set; }
    }

    public class PrediccionResultado
    {
        [ColumnName("Score")]
        public float Prediccion { get; set; }
    }
}
