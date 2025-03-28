namespace Prolimza.Models
{
    public class DetalleCompraMateriaPrima
    {
        public int IdCompra { get; set; }
        public Compra Compra { get; set; }
        public int IdMateriaPrima { get; set; }
        public MateriaPrima MateriaPrima { get; set; }
        public int Cantidad { get; set; }
    }
}
