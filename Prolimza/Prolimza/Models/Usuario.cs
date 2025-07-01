namespace Prolimza.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string contrasenaEncriptada { get; set; }
        public int IdRol { get; set; } = 4;
        public Rol? Rol { get; set; }
        public ICollection<Auditoria>? Auditorias { get; set; }
        public ICollection<Alerta>? Alertas { get; set; }

    }
}
