namespace Prolimza.Models
{
    public class Alerta
    {
        public int IdAlerta { get; set; }

        public string Tipo { get; set; } // Ej: 'Stock', 'Caducidad', 'Sistema', 'Auditoria'

        public string Descripcion { get; set; }

        public DateTime FechaAlerta { get; set; }

        public string Estado { get; set; } // 'Pendiente', 'Revisada', 'Resuelta'

        public int? IdUsuario { get; set; }

        public Usuario Usuario { get; set; }
    }
}
