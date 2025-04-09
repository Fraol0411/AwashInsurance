using AwashInsurance.Models;
using AwashInsurance.Services;
using Microsoft.AspNetCore.Mvc;

namespace AwashInsurance.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _service;

        public EmployeeController(IEmployeeService service)
        {
            _service = service;
        }

        public IActionResult Index(string searchName)
        {
            var employees = string.IsNullOrEmpty(searchName)
                ? _service.GetAllEmployees()
                : _service.SearchEmployees(searchName);

            return View(employees);
        }

        // Show form
        public IActionResult Create()
        {
            return View();
        }

        // Handle form submission
        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _service.Add(employee);
                return RedirectToAction("Index"); // or wherever you list employees
            }
            return View(employee);
        }
    }

}
