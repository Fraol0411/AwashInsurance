namespace AwashInsurance.Models
{
    public class Employee
    {
        public int Id { get; set; }

        // Essential Fields
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        // Foreign Key to OrganizationUnit
        public int OrganizationUnitId { get; set; }
        public OrganizationUnit OrganizationUnit { get; set; }

        public ICollection<UserAccount> UserAccounts { get; set; }
    }


}
