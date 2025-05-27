namespace Prolimza.Models
{
    public class DetalleCompraProducto
    {
        public int IdCompra { get; set; }
        public Compra? Compra { get; set; }
        public int IdProducto { get; set; }
        public Producto? Producto { get; set; }
        public int Cantidad { get; set; }
    }
}
