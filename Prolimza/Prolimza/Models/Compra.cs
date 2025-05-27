namespace Prolimza.Models
{
    public class Compra
    {
        public int IdCompra { get; set; }
        public DateTime FechaCompra { get; set; }
        public int IdUsuario { get; set; }
        public Usuario? Usuario { get; set; }
        public ICollection<DetalleCompraProducto>? DetalleCompraProductos { get; set; }
        public ICollection<DetalleCompraMateriaPrima>? DetalleCompraMateriaPrimas { get; set; }
    }
}
