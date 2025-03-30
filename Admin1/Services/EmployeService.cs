using BenjaminCamacho.Models;

namespace BenjaminCamacho.Services
{
    public class EmployeeServices : IEmployeeService
    {
        public IList<Employee> GetAll()
        {
            List<Employee> empleados = new List<Employee>();
            var lineas = File.ReadAllLines("empleados.csv");
            foreach (var linea in lineas)
            {
                if (linea.Length > 0)
                {
                    var valores = linea.Split(',');
                    Employee empleado = new Employee();
                    empleado.Name = valores[0];
                    empleado.Position = valores[1];
                    empleado.Office = valores[2];
                    empleado.Age = valores[3];
                    empleado.StartDate = valores[4];
                    empleado.Salary = valores[5];
                    empleados.Add(empleado);
                }
            }
            return empleados;
        }
    }
}