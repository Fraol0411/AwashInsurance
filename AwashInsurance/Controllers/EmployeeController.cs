using Microsoft.AspNetCore.Mvc;
using AwashInsurance.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AwashInsurance.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }

        // ------------------------------
        // ADD EMPLOYEE
        // ------------------------------

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.OrganizationUnits = _context.OrganizationUnits.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Employee employee)
        {
            if (true)
            {
                try
                {
                    _context.Employees.Add(employee);
                    _context.SaveChanges();

                    TempData["Success"] = "Employee added successfully!";
                    ViewBag.OrganizationUnits = _context.OrganizationUnits.ToList();
                    return View();
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error saving data: " + ex.Message;
                }
            }
            else
            {
                TempData["Error"] = "Form validation failed. Please check all required fields.";
            }

            ViewBag.OrganizationUnits = _context.OrganizationUnits.ToList();
            return View(employee);
        }

        // ------------------------------
        // MODIFY EMPLOYEE
        // ------------------------------

        [HttpGet]
        public IActionResult Modify()
        {
            ViewBag.OrganizationUnits = _context.OrganizationUnits.ToList();
            return View(new List<Employee>());
        }

        [HttpPost]
        public IActionResult Modify(Employee updatedEmployee)
        {
            var existingEmployee = _context.Employees.FirstOrDefault(e => e.Id == updatedEmployee.Id);

            if (existingEmployee == null)
            {
                TempData["Error"] = "Employee not found.";
                ViewBag.OrganizationUnits = _context.OrganizationUnits.ToList();
                return View(_context.Employees.ToList());
            }

            // Update fields
            existingEmployee.FirstName = updatedEmployee.FirstName;
            existingEmployee.LastName = updatedEmployee.LastName;
            existingEmployee.Gender = updatedEmployee.Gender;
            existingEmployee.PhoneNumber = updatedEmployee.PhoneNumber;
            existingEmployee.Email = updatedEmployee.Email;
            existingEmployee.OrganizationUnitId = updatedEmployee.OrganizationUnitId;

            _context.SaveChanges();

            TempData["Success"] = "Employee information updated successfully!";
            ViewBag.Selected = existingEmployee;
            ViewBag.OrganizationUnits = _context.OrganizationUnits.ToList();

            return View(new List<Employee>());
        }

        // ------------------------------
        // SEARCH EMPLOYEE
        // ------------------------------

        public IActionResult SearchEmployee(string? searchBy, string? searchValue, string? returnView)
        {
            if (string.IsNullOrWhiteSpace(searchBy) || string.IsNullOrWhiteSpace(searchValue))
                return View(returnView ?? "Modify", new List<Employee>());

            var employees = _context.Employees.AsQueryable();

            if (searchBy == "Id")
            {
                if (int.TryParse(searchValue, out int id))
                {
                    employees = employees.Where(e => e.Id == id);
                }
                else
                {
                    TempData["Error"] = "Please enter a valid numeric Employee ID.";
                    return View(returnView ?? "Modify", new List<Employee>());
                }
            }
            else if (searchBy == "FirstName")
            {
                employees = employees.Where(e =>
                    e.FirstName.Contains(searchValue));
                if(employees.Count() == 0)
                {
                    TempData["Error"] = "No employee found with the given first name.";
                    return View(returnView ?? "Modify", new List<Employee>());
                }

            }

            var result = employees.ToList();
            TempData["SearchResult"] = System.Text.Json.JsonSerializer.Serialize(result);

            return View(returnView ?? "Modify", result);
        }

        // ------------------------------
        // SELECT EMPLOYEE FROM RESULT
        // ------------------------------

        [HttpPost]
        public IActionResult SelectEmployee(int selectedEmployeeId, string? returnView)
        {
            //if (TempData["SearchResult"] == null)
            //    return RedirectToAction(returnView ?? "Modify");

            var employeeList = System.Text.Json.JsonSerializer.Deserialize<List<Employee>>(TempData["SearchResult"].ToString());
            var selectedEmployee = _context.Employees
            .Include(e => e.OrganizationUnit)
           .FirstOrDefault(e => e.Id == selectedEmployeeId);


            if (selectedEmployee == null)
            {
                TempData["Error"] = "Selected employee not found.";
                return View(returnView ?? "Modify", employeeList);
            }

            ViewBag.SelectedEmployee = selectedEmployee;
            ViewBag.OrganizationUnits = _context.OrganizationUnits.ToList();
            TempData["SearchResult"] = System.Text.Json.JsonSerializer.Serialize(employeeList);

            return View(returnView ?? "Modify", employeeList);
        }

        // ------------------------------
        // DELETE EMPLOYEE
        // ------------------------------

        [HttpGet]
        public IActionResult Delete()
        {
            return View(new List<Employee>());
        }

        [HttpPost]
        public IActionResult Delete(string EmployeeId, string RemovalReason)
        {
            if (string.IsNullOrWhiteSpace(EmployeeId))
            {
                TempData["Error"] = "Invalid employee ID. Cannot proceed with deletion.";
                return View(new List<Employee>());
            }

            if (!int.TryParse(EmployeeId, out int id))
            {
                TempData["Error"] = "Employee ID must be a valid number.";
                return View(new List<Employee>());
            }

            try
            {
                var employee = _context.Employees.FirstOrDefault(e => e.Id == id);
                if (employee == null)
                {
                    TempData["Error"] = "Employee not found.";
                    return View(new List<Employee>());
                }

                // Optional: Log reason
                Console.WriteLine($"Employee deleted successfully. Reason: {RemovalReason}");

                _context.Employees.Remove(employee);
                _context.SaveChanges();

                TempData["Success"] = $"Employee (ID: {id}) deleted successfully. Reason: {RemovalReason}";
                return View(new List<Employee>());
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error deleting employee: " + ex.Message;
                return View(new List<Employee>());
            }
        }

        // ------------------------------
        // INQUIRE EMPLOYEE
        // ------------------------------

        public IActionResult Inquire()
        {
            return View(new List<Employee>());
        }
    }
}