namespace Prolimza.Models
{
    public class MateriaPrima
    {
        public int IdMateriaPrima { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Sku { get; set; }
        public int Cantidad { get; set; }
        public DateTime? FechaCaducidad { get; set; }
        public int IdBodega { get; set; }
        public Bodega Bodega { get; set; }
    }
}
