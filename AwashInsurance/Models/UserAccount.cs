using System.ComponentModel.DataAnnotations.Schema;

namespace AwashInsurance.Models
{
    public class UserAccount
    {
        public int Id { get; set; }  // PK

        public string LoginId { get; set; }  // Unique
        public string Password { get; set; }

        // Foreign Keys
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;

        [NotMapped] // This is just for display use
        public string EmployeeName => $"{Employee?.FirstName} {Employee?.LastName}";
    }

}
