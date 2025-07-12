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
    public class ReporteUsoMateriaPrimaViewModel
    {
        public int IdVenta { get; set; }
        public DateTime FechaVenta { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public string TipoProducto { get; set; } = string.Empty;
        public string? NombreMateriaPrima { get; set; }
        public int? CantidadMateriaPrimaUsada { get; set; }
        public int CantidadProductoVendido { get; set; }
    }

}
