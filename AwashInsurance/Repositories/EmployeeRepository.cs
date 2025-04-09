using AwashInsurance.Models;

namespace AwashInsurance.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly YourDbContextName _context;

        public EmployeeRepository(YourDbContextName context)
        {
            _context = context;
        }

        public IEnumerable<Employee> GetAll()
        {
            return _context.Employees.ToList();
        }

        public IEnumerable<Employee> SearchByName(string name)
        {
            return _context.Employees
                           .Where(e => e.FirstName.Contains(name))
                           .ToList();
        }
        public void Add(Employee employee)
        {
            _context.Employees.Add(employee);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

    }

}
