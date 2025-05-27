namespace Prolimza.Models
{
    public class Rol
    {
        public int IdRol { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public ICollection<Usuario>? Usuarios { get; set; }
    }
}
