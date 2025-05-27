namespace Prolimza.Models
{
    public class Auditoria
    {
        public int IdAuditoria { get; set; }
        public string Log { get; set; }
        public string TipoEvento { get; set; }
        public int? IdProducto { get; set; }
        public Producto? Producto { get; set; }
        public int? IdMateriaPrima { get; set; }
        public MateriaPrima? MateriaPrima { get; set; }
    }
}
