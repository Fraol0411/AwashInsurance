// Models/Customer.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace AwashInsurance.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        public string FullName { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; }

        public string IDType { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string CustomerType { get; set; }

        public bool IsSubscribedToNewsletter { get; set; }

        [DataType(DataType.Date)]
        public DateTime RegistrationDate { get; set; }

        public string Notes { get; set; }
    }
}
