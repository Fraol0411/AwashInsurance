using AwashInsurance.Models;

namespace AwashInsurance.Models
{
    public class OrganizationUnit
    {
        public int Id { get; set; }
        public string Name { get; set; } 

        public int OrganizationTypeId { get; set; }
        public OrganizationType OrganizationType { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }

}



