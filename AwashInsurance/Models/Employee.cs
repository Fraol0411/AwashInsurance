// Models/Employee.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace AwashInsurance.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Phone { get; set; }

        public string Position { get; set; }

        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }

        public bool IsActive { get; set; }
    }
}
