using System.ComponentModel.DataAnnotations;

namespace BenjaminCamacho.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }

}
