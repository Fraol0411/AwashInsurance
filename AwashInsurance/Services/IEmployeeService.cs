using AwashInsurance.Models;

namespace AwashInsurance.Services
{
    public interface IEmployeeService
    {
        IEnumerable<Employee> GetAllEmployees();
        IEnumerable<Employee> SearchEmployees(string name);

        void Add(Employee employee);


    }
}
