using AwashInsurance.Models;

namespace AwashInsurance.Repositories
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAll();
        IEnumerable<Employee> SearchByName(string name);

        void Add(Employee employee);
        void Save();
    }
}
