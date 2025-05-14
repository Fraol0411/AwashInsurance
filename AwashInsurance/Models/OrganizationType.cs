using AwashInsurance.Models;

namespace AwashInsurance.Models
{
    public class OrganizationType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<OrganizationUnit> OrganizationUnits { get; set; }
    }

}



