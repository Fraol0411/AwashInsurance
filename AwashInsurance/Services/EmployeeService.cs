using AwashInsurance.Models;
using AwashInsurance.Repositories;

namespace AwashInsurance.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _repository.GetAll();
        }

        public IEnumerable<Employee> SearchEmployees(string name)
        {
            return _repository.SearchByName(name);
        }


        public void Add(Employee employee)
        {
            _repository.Add(employee);
            _repository.Save();
        }
    }

}
