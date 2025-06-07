namespace Prolimza.Models
{
    public class Auditoria
    {
        public int IdAuditoria { get; set; }
        public string Log { get; set; }
        public string TipoEvento { get; set; }
        public DateTime FechaEvento { get; set; }

        // Relación con Usuario
        public int IdUsuario { get; set; }
        public Usuario? Usuario { get; set; }
    }
}
