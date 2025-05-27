namespace Prolimza.Models
{
    public class EstadoVenta
    {
        public int IdEstadoVenta { get; set; }
        public string Descripcion { get; set; }
        public ICollection<HistorialEstadoVenta>? HistorialesEstadoVenta { get; set; }
    }
}
