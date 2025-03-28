namespace Prolimza.Models
{
    public class Provincia
    {
        public int IdProvincia { get; set; }
        public string Nombre { get; set; }
        public ICollection<Canton> Cantones { get; set; }
    }
}
