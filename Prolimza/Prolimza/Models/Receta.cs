namespace Prolimza.Models
{
    public class Receta
    {
        public int IdReceta { get; set; }
        public int IdProducto { get; set; }
        public Producto? Producto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public ICollection<MateriaReceta>? MateriasReceta { get; set; }
    }

}
