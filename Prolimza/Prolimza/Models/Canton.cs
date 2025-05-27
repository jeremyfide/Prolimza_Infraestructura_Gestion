namespace Prolimza.Models
{
    public class Canton
    {
        public int IdCanton { get; set; }
        public string Nombre { get; set; }
        public int IdProvincia { get; set; }


        public Provincia? Provincia { get; set; }
        public ICollection<Distrito>? Distritos { get; set; }

    }
}
