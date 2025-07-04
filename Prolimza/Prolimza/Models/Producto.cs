namespace Prolimza.Models
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public string Descripcion { get; set; }
        public string Sku { get; set; }
        public int Cantidad { get; set; }
        public DateTime? FechaCaducidad { get; set; }
        public int IdBodega { get; set; }
        public int stockMinimo { get; set; }

        public Bodega? Bodega { get; set; }

        public ICollection<Receta>? Recetas { get; set; } = new List<Receta>();
        public ICollection<DetalleCompraProducto>? DetallesCompraProductos { get; set; }

    }
}
