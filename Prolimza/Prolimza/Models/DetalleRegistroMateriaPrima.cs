using System.ComponentModel.DataAnnotations;

namespace Prolimza.Models
{
    public class DetalleRegistroMateriaPrima
    {
        [Key]
        public int IdDetalle { get; set; }

        public int IdRegistro { get; set; }
        public int IdMateriaPrima { get; set; }
        public string NombreMateriaPrima { get; set; }
        public int CantidadConsumida { get; set; }

        public RegistroCadenaProduccion Registro { get; set; }
    }
}
