using AwashInsurance.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AwashInsurance.Controllers
{
    public class EmployeeController : Controller
    {



        private readonly YourDbContextName _context;

        public EmployeeController(YourDbContextName context)
        {
            _context = context;
        }

        // GET: Show the form
        [HttpGet]
        public IActionResult Create()
        {
            return View(); // sends empty Employee to the form
        }

        // POST: Save data to DB
        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index"); // show success list
            }

            return View(employee); // re-render form with errors
        }

        // View all employees (for redirect after submit)
        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees.ToListAsync();
            return View(employees);
        }


        [HttpGet]
        public async Task<IActionResult> Search(string searchName)
        {
            List<Employee> results;

            if (string.IsNullOrWhiteSpace(searchName))
            {
                results = new List<Employee>();
                ViewBag.Message = "Please enter a name.";
            }
            else
            {
                results = await _context.Employees
                    .Where(e => e.FirstName.Contains(searchName) || e.LastName.Contains(searchName))
                    .ToListAsync();

                if (!results.Any())
                {
                    ViewBag.Message = "No employees found.";
                }
            }

            // ✅ Return to the Modify view (NOT Search)
            return View("Index",results);
        }


    }
}
