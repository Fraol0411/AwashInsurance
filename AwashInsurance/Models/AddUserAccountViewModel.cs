// Models/AddUserAccountViewModel.cs
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AwashInsurance.Models
{
    public class AddUserAccountViewModel
    {
        public List<Employee> Employees { get; set; } = new List<Employee>();
        public SelectList Roles { get; set; }
        public Employee SelectedEmployee { get; set; }
        public int RoleId { get; set; }
        public string LoginId { get; set; }
        public string Password { get; set; }

        public bool IsActive { get; set; } = true; // Default to "True" for active
    }
}