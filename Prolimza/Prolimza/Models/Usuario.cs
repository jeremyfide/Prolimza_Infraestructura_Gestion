namespace Prolimza.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string ContrasenaEncriptada { get; set; }
        public int IdRol { get; set; } = 1;
        public Rol? Rol { get; set; }
    }
}
