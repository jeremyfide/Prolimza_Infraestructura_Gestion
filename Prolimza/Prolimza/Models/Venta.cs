namespace Prolimza.Models
{
    public class Venta
    {
        public int IdVenta { get; set; }
        public DateTime FechaVenta { get; set; }
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
        public ICollection<DetalleVenta> DetallesVenta { get; set; }
    }
}
