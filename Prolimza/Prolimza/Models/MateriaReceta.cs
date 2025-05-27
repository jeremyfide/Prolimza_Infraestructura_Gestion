namespace Prolimza.Models
{
    public class MateriaReceta
    {
        public int IdReceta { get; set; }
        public Receta? Receta { get; set; }
        public int IdMateriaPrima { get; set; }
        public MateriaPrima? MateriaPrima { get; set; }
        public int Cantidad { get; set; }
    }
}
