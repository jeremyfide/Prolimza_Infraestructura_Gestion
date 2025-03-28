namespace Prolimza.Models
{
    public class DetalleVenta
    {
        public int IdVenta { get; set; }
        public Venta Venta { get; set; }
        public int IdProducto { get; set; }
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }
    }
}
