using System.ComponentModel.DataAnnotations;

namespace Prolimza.Models
{
    public class Compra
    {
        public int IdCompra { get; set; }

        public DateTime FechaCompra { get; set; }

        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El campo Código de Ingreso es obligatorio.")]
        public string CodigoIngreso { get; set; }

        public string Estado { get; set; } = "Pendiente";

        public Usuario? Usuario { get; set; }

        public ICollection<DetalleCompraProducto>? DetalleCompraProductos { get; set; }

        public ICollection<DetalleCompraMateriaPrima>? DetalleCompraMateriaPrimas { get; set; }

        public Compra()
        {
            FechaCompra = DateTime.Now;
        }
    }
}
