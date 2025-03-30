using System.ComponentModel.DataAnnotations;

namespace BenjaminCamacho.Models
{
    public class Employee
    {
        public string? Name { get; set; }
        public string? Position { get; set; }
        public string? Office { get; set; }
        public string? Age { get; set; }

        [Display(Name = "Start date")]
        public string? StartDate { get; set; }
        public string? Salary { get; set; }
    }
}
