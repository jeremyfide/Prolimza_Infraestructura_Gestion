using BenjaminCamacho.Models;
using BenjaminCamacho.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BenjaminCamacho.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IEmployeeService employeeServices;

        public IList<Employee> Employees { get; set; } = default!;

        public IndexModel(IEmployeeService employeeServices)
        {
            this.employeeServices = employeeServices;
        }

        public void OnGet()
        {
            Employees = employeeServices.GetAll().ToList();
        }
    }
}