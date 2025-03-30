using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BenjaminCamacho.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class User : IdentityUser
    {
        [Required]
        [Display(Name = "FirstName")]
        public string? FirstName { get; set; }

        [Required]
        [Display(Name = "LastName")]
        public string? LastName { get; set; }
    }
}