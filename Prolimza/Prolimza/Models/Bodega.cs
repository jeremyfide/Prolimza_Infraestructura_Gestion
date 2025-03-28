namespace Prolimza.Models
{
    public class Bodega
    {
        public int IdBodega { get; set; }
        public string Nombre { get; set; }
        public string DetalleDireccion { get; set; }
        public int IdDistrito { get; set; }
        public Distrito Distrito { get; set; }
        public ICollection<MateriaPrima> MateriasPrimas { get; set; }
        public ICollection<Producto> Productos { get; set; }
    }

}
