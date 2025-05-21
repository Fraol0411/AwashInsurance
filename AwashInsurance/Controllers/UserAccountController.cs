using Microsoft.AspNetCore.Mvc;
using AwashInsurance.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using AwashInsurance.Models; // Make sure this exists

namespace AwashInsurance.Controllers
{
    public class UserAccountController : Controller
    {
        private readonly AppDbContext _context;

        public UserAccountController(AppDbContext context)
        {
            _context = context;
        }





        // ------------------------------
        // SEARCH EMPLOYEE
        // ------------------------------

        [HttpGet]
        public IActionResult SearchEmployee(string searchBy, string searchValue, string returnView)
        {
            var employees = new List<Employee>();

            if (!string.IsNullOrEmpty(searchValue))
            {
                if (searchBy == "Id")
                {
                    //employees = _context.Employees
                    //    .Where(e => e.Id.Contains(searchValue))
                    //    .ToList();
                }
                else if (searchBy == "FirstName")
                {
                    employees = _context.Employees
                        .Where(e => e.FirstName.Contains(searchValue))
                        .ToList();
                }
            }

            var viewModel = new AddUserAccountViewModel
            {
                Employees = employees,
                Roles = new SelectList(_context.Roles.ToList(), "Id", "Name")
            };

            return View(returnView, viewModel); // "Add" or other view name
        }



        // ------------------------------
        // SELECT EMPLOYEE FROM RESULT
        // ------------------------------

        [HttpPost]
        public IActionResult SelectEmployee(int selectedEmployeeId, string returnView)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.Id == selectedEmployeeId);
            var userAccounts = _context.UserAccounts
                .Where(u => u.EmployeeId == selectedEmployeeId)
                .Include(u => u.Role) // Include role for display
                .ToList();

            var viewModel = new AddUserAccountViewModel
            {
                SelectedEmployee = employee,
                UserAccounts = userAccounts,
                Employees = new List<Employee> { employee },
                Roles = new SelectList(_context.Roles.ToList(), "Id", "Name")
            };

            return View(returnView, viewModel);
        }






        // ------------------------------
        // SELECT USERACCOUNT FOR EMPLOYEE FROM RESULT
        // ------------------------------
        [HttpPost]
        public IActionResult SelectUserAccount(int employeeId, int selectedAccountId, string returnView)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.Id == employeeId);
            var userAccounts = _context.UserAccounts
                .Where(u => u.EmployeeId == employeeId)
                .Include(u => u.Role)
                .ToList();

            var selectedAccount = userAccounts.FirstOrDefault(u => u.Id == selectedAccountId);

            var viewModel = new AddUserAccountViewModel
            {
                SelectedEmployee = employee,
                UserAccounts = userAccounts,
                SelectedAccount = selectedAccount,
                Employees = new List<Employee> { employee },
                Roles = new SelectList(_context.Roles.ToList(), "Id", "Name"),
                SelectedAccountId = selectedAccountId,
                LoginId = selectedAccount.LoginId,
                RoleId = selectedAccount.RoleId,
                IsActive = selectedAccount.IsActive
            };

            return View(returnView, viewModel);
        }




        // ------------------------------
        // ADD EMPLOYEE
        // ------------------------------

        [HttpGet]
        public IActionResult Add()
        {
            var viewModel = new AddUserAccountViewModel
            {
                Roles = new SelectList(_context.Roles.ToList(), "Id", "Name")
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddUserAccountViewModel model)
        {
            try
            {
                //if (!ModelState.IsValid)
                //{
                //    TempData["Error"] = "Form validation failed. Please check all required fields.";
                //    model.Roles = new SelectList(_context.Roles.ToList(), "Id", "Name");
                //    return View(model);
                //}

                // Create the UserAccount
                var userAccount = new UserAccount
                {
                    LoginId = model.LoginId,
                    Password = model.Password, 
                    EmployeeId = model.SelectedEmployee.Id,
                    RoleId = model.RoleId,
                    CreatedAt = DateTime.Now,
                    IsActive = true
                };

                _context.UserAccounts.Add(userAccount);
                await _context.SaveChangesAsync();

                TempData["Success"] = "User account created successfully!";
                return RedirectToAction(nameof(Add)); // PRG pattern
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error creating user account: " + ex.Message;
                model.Roles = new SelectList(_context.Roles.ToList(), "Id", "Name");
                return View(model);
            }
        }


      






        // ------------------------------
        // MODIFY EMPLOYEE
        // ------------------------------

        [HttpGet]
        public IActionResult Modify()
        {
            var viewModel = new AddUserAccountViewModel
            {
                Roles = new SelectList(_context.Roles.ToList(), "Id", "Name")
            };
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Modify(AddUserAccountViewModel model)
        {
            try
            {
                var existingAccount = await _context.UserAccounts
                    .FirstOrDefaultAsync(u => u.Id == model.UserAccountId);


                if (existingAccount == null)
                {
                    TempData["Error"] = "User account not found";
                    return RedirectToAction(nameof(Modify));
                }

                // Update the account
                existingAccount.LoginId = model.LoginId;
                existingAccount.RoleId = model.RoleId;
                existingAccount.IsActive = model.IsActive;

                // Only update password if provided
                if (!string.IsNullOrEmpty(model.Password))
                {
                    existingAccount.Password = model.Password;
                }

                _context.UserAccounts.Update(existingAccount);
                await _context.SaveChangesAsync();

                TempData["Success"] = "User account updated successfully!";
                return RedirectToAction(nameof(Modify));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error updating user account: " + ex.Message;
                model.Roles = new SelectList(_context.Roles.ToList(), "Id", "Name");
                return View(model);
            }
        }





        // ------------------------------
        // DELETE EMPLOYEE
        // ------------------------------

        [HttpGet]
        public IActionResult Delete()
        {
            var viewModel = new AddUserAccountViewModel
            {
                Roles = new SelectList(_context.Roles.ToList(), "Id", "Name")
            };
            return View(viewModel);
        }
        

        [HttpPost]
        public async Task<IActionResult> Delete(AddUserAccountViewModel model)
        {

            try
            {
                var account = _context.UserAccounts.FirstOrDefault(e => e.Id == model.UserAccountId);

                if (account == null)
                {
                    TempData["Error"] = "UserAccount not found.";
                    return View(new List<Employee>());
                }

                // Optional: Log reason
                Console.WriteLine($"UserAccount deleted successfully. Reason: ");

                _context.UserAccounts.Remove(account);
                _context.SaveChanges();

                TempData["Success"] = $"UserAccount (ID: {model.UserAccountId}) deleted successfully. Reason:";
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error deleting UserAccount: " + ex.Message;
                return View(model);
            }
        }




        // ------------------------------
        // INQUIRE EMPLOYEE
        // ------------------------------

        public IActionResult Inquire()
        {
            var viewModel = new AddUserAccountViewModel
            {
                Roles = new SelectList(_context.Roles.ToList(), "Id", "Name")
            };
            return View(viewModel);
        }
    }
}