using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Prolimza.Models
{
    public class Venta
    {
        public int IdVenta { get; set; }
        public DateTime FechaVenta { get; set; }
        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }
        public Usuario? Usuario { get; set; }
        [Required(ErrorMessage = "El campo Código de Ingreso es obligatorio.")]
        public string CodigoIngreso { get; set; } = string.Empty;
        public ICollection<DetalleVenta>? DetallesVenta { get; set; } = new List<DetalleVenta>();
        public ICollection<HistorialEstadoVenta>? HistorialesEstadoVenta { get; set; }

    }
}
