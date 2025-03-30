using BenjaminCamacho.Models;
using BenjaminCamacho.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BenjaminCamacho.Pages
{
    public class TablesModel : PageModel
    {
        private readonly IEmployeeService employeeServices;

        public IList<Employee> Employees { get; set; } = default!;

        public TablesModel(IEmployeeService employeeServices)
        {
            this.employeeServices = employeeServices;
        }

        public void OnGet()
        {
            Employees = employeeServices.GetAll().ToList();
        }
    }
}