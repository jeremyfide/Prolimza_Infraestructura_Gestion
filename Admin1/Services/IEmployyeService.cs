using BenjaminCamacho.Models;

namespace BenjaminCamacho.Services
{
    public interface IEmployeeService
    {
        IList<Employee> GetAll();
    }
}