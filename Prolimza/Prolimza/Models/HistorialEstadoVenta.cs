namespace Prolimza.Models
{
    public class HistorialEstadoVenta
    {
        public int IdHistorialEstadoVenta { get; set; }
        public int IdVenta { get; set; }
        public Venta? Venta { get; set; }
        public int IdEstadoVenta { get; set; }
        public EstadoVenta? EstadoVenta { get; set; }
        public DateTime FechaEstado { get; set; }
    }
}
