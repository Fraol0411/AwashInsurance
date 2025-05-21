// Models/AddUserAccountViewModel.cs
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AwashInsurance.Models
{
    // AddUserAccountViewModel.cs
    public class AddUserAccountViewModel
    {
        public List<Employee> Employees { get; set; } = new List<Employee>();
        public SelectList Roles { get; set; }
        public Employee SelectedEmployee { get; set; }

        public int EmployeeId { get; set; } // Instead of just SelectedEmployee.Id


        public List<UserAccount> UserAccounts { get; set; } = new List<UserAccount>(); // 🔥 New
        public UserAccount SelectedAccount { get; set; } // 🔥 New

        public int SelectedAccountId { get; set; } // 🔥 New: To track which one is selected

        public int UserAccountId { get; set; }
        public int RoleId { get; set; }
        public string LoginId { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; } = true;
    }

}