using Microsoft.AspNetCore.Mvc.Rendering;

namespace Prolimza.Models
{
    public class RecetasPorDiaViewModel
    {
        public DateTime Fecha { get; set; }
        public int idReceta { get; set; }
        public string NombreReceta { get; set; }
        public int CantidadProductosVendidos { get; set; }
    }

}
