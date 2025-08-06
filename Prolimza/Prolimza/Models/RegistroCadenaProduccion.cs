using System.ComponentModel.DataAnnotations;

namespace Prolimza.Models
{
    public class RegistroCadenaProduccion
    {

        [Key]

        public int IdRegistro { get; set; }
        public string Sku { get; set; }
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public int CantidadFinal { get; set; }
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int? IdUsuario { get; set; }

        public ICollection<DetalleRegistroMateriaPrima> Detalles { get; set; }
    }

}
