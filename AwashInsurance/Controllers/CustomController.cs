using Microsoft.AspNetCore.Mvc;
using AwashInsurance.Models;

namespace AwashInsurance.Controllers
{
    public class CustomController : Controller
    {
        private readonly AppDbContext _context;

        public CustomController(AppDbContext context)
        {
            _context = context;
        }

        // ------------------------------
        // ADD CUSTOMER
        // ------------------------------

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Customer customer)
        {
            if (true)
            {
                try
                {
                    _context.Customers.Add(customer);
                    _context.SaveChanges();
                    TempData["Success"] = "Employee added successfully!";
                    return RedirectToAction("Add"); // Optional redirect to clear form
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error saving data: " + ex.Message;
                }
            }
            else
            {
                TempData["Error"] = "Form validation failed. Please check all required fields.";
                foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(modelError.ErrorMessage); // Optional debugging
                }
            }

            return View(customer);
        }


        // ------------------------------
        // MODIFY CUSTOMER
        // ------------------------------

        [HttpGet]
        public IActionResult Modify()
        {
            return View(new List<Customer>());
        }

        [HttpPost]
        public IActionResult Modify(Customer updatedCustomer)
        {
            var existingCustomer = _context.Customers.FirstOrDefault(c => c.CustomerId == updatedCustomer.CustomerId);

            if (existingCustomer == null)
            {
                TempData["Error"] = "Customer not found.";
                return View(_context.Customers.ToList());
            }

            // Update fields
            existingCustomer.FullName = updatedCustomer.FullName;
            existingCustomer.DateOfBirth = updatedCustomer.DateOfBirth;
            existingCustomer.Gender = updatedCustomer.Gender;
            existingCustomer.IDType = updatedCustomer.IDType;
            existingCustomer.Email = updatedCustomer.Email;
            existingCustomer.Phone = updatedCustomer.Phone;
            existingCustomer.Address = updatedCustomer.Address;
            existingCustomer.City = updatedCustomer.City;
            existingCustomer.CustomerType = updatedCustomer.CustomerType;
            existingCustomer.RegistrationDate = updatedCustomer.RegistrationDate;
            existingCustomer.IsSubscribedToNewsletter = updatedCustomer.IsSubscribedToNewsletter;
            existingCustomer.Notes = updatedCustomer.Notes;

            _context.SaveChanges();

            TempData["Success"] = "Customer information updated successfully!";
            ViewBag.Selected = existingCustomer;

            return View(new List<Customer>());
        }

        // ------------------------------
        // SEARCH CUSTOMER
        // ------------------------------

        public IActionResult SearchUser(string? searchBy, string? searchValue, string? returnView)
        {
            if (string.IsNullOrWhiteSpace(searchBy) || string.IsNullOrWhiteSpace(searchValue))
                return View(returnView ?? "Modify", new List<Customer>());

            var customers = _context.Customers.AsQueryable();

            if (searchBy == "CustomerId")
            {
                if (int.TryParse(searchValue, out int id))
                {
                    customers = customers.Where(c => c.CustomerId == id);
                }
                else
                {
                    TempData["Error"] = "Please enter a valid numeric Customer ID.";
                    return View(returnView ?? "Modify", new List<Customer>());
                }
            }
            else if (searchBy == "CustomerName")
            {
                customers = customers.Where(c => c.FullName.Contains(searchValue));
            }

            var result = customers.ToList();
            TempData["SearchResult"] = System.Text.Json.JsonSerializer.Serialize(result);

            return View(returnView ?? "Modify", result);
        }

        // ------------------------------
        // SELECT CUSTOMER FROM RESULT
        // ------------------------------

        [HttpPost]
        public IActionResult SelectCustomer(int selectedCustomerId, string? returnView)
        {
            if (TempData["SearchResult"] == null)
                return RedirectToAction(returnView ?? "Modify");

            var customerList = System.Text.Json.JsonSerializer.Deserialize<List<Customer>>(TempData["SearchResult"].ToString());
            var selectedCustomer = customerList.FirstOrDefault(c => c.CustomerId == selectedCustomerId);

            if (selectedCustomer == null)
            {
                TempData["Error"] = "Selected customer not found.";
                return View(returnView ?? "Modify", customerList);
            }

            ViewBag.SelectedCustomer = selectedCustomer;
            TempData["SearchResult"] = System.Text.Json.JsonSerializer.Serialize(customerList); // Re-store TempData

            return View(returnView ?? "Modify", customerList);
        }

        // ------------------------------
        // DELETE CUSTOMER
        // ------------------------------

        [HttpGet]
        public IActionResult Delete()
        {
            return View(new List<Customer>());
        }

        [HttpPost]
        public IActionResult Delete(string CustomerId, string RemovalReason)
        {
            if (string.IsNullOrWhiteSpace(CustomerId))
            {
                TempData["Error"] = "Invalid customer ID. Cannot proceed with deletion.";
                return View(new List<Customer>());
            }

            if (!int.TryParse(CustomerId, out int id))
            {
                TempData["Error"] = "Customer ID must be a valid number.";
                return View(new List<Customer>());
            }

            try
            {
                var customer = _context.Customers.FirstOrDefault(c => c.CustomerId == id);
                if (customer == null)
                {
                    TempData["Error"] = "Customer not found.";
                    return View(new List<Customer>());
                }

                // Optional: Log reason
                Console.WriteLine($"Customer deleted successfully. Reason: {RemovalReason}");

                _context.Customers.Remove(customer);
                _context.SaveChanges();

                TempData["Success"] = $"Customer (ID: {id}) deleted successfully. Reason: {RemovalReason}";
                return View(new List<Customer>());
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error deleting customer: " + ex.Message;
                return View(new List<Customer>());
            }
        }

        // ------------------------------
        // INQUIRE CUSTOMER
        // ------------------------------

        public IActionResult Inquire()
        {
            return View(new List<Customer>());
        }
    }
}
