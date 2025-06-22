using Microsoft.AspNetCore.Mvc.Rendering;

namespace Prolimza.Models
{
    public class CompraCreateViewModel
    {
        public Compra Compra { get; set; } = new();
        public List<DetalleCompraProducto>? Productos { get; set; } = new();
        public List<DetalleCompraMateriaPrima>? MateriasPrimas { get; set; } = new();

        public List<SelectListItem> ListaProductos { get; set; } = new();
        public List<SelectListItem> ListaMateriasPrimas { get; set; } = new();
    }
}
