using Microsoft.AspNetCore.Mvc;
using AwashInsurance.Models;


namespace AwashInsurance.Controllers
{
    public class CustomerController : Controller
    {
        private readonly AppDbContext _context;

        public CustomerController(AppDbContext context)
        {
            _context = context;
        }



        
                 // ADDING LOGIC      
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Customers.Add(customer);
                    _context.SaveChanges();

                    TempData["Success"] = "Customer added successfully!";
                    //return RedirectToAction("Add"); // or wherever you want to redirect
                    return View();
                }
                catch (Exception ex)
                {
                    // Log the error if needed
                    TempData["Error"] = "Error saving data: " + ex.Message;
                }
            }
            else
            {
                TempData["Error"] = "Form validation failed. Please check all required fields.";
            }

            return View(customer);
        }



               // Modify LOGIC  
        // GET: Load the view initially
        [HttpGet]
        public IActionResult Modify()
        {
            return View(new List<Customer>()); // returns the empty form
        }

       
        // POST: Save updates to the selected customer
        [HttpPost]
        public IActionResult Modify(Customer updatedCustomer)
        {
            //if (!ModelState.IsValid)
            //{
            //    TempData["Error"] = "Please correct the errors in the form.";

            //    // Fetch the full customer list again
            //    var allCustomers = _context.Customers.ToList();

            //    // Set the selected customer for editing
            //    ViewBag.Selected = updatedCustomer;

            //    // Return the list so view can render
            //    return View(allCustomers);
            //}

            var existingCustomer = _context.Customers.FirstOrDefault(c => c.CustomerId == updatedCustomer.CustomerId);

            if (existingCustomer == null)
            {
                TempData["Error"] = "Customer not found.";
                var allCustomers = _context.Customers.ToList();
                return View(allCustomers);
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

            // After saving, return to the same view with updated list and selected customer
            var customers = _context.Customers.ToList();
            ViewBag.Selected = existingCustomer;
            return View(new List<Customer>());
        }




        // Search the User Method
        public IActionResult SearchUser(string? searchBy, string? searchValue)
        {
            if (string.IsNullOrWhiteSpace(searchBy) || string.IsNullOrWhiteSpace(searchValue))
            {
                return View("Modify", new List<Customer>());
            }

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
                    return View("Modify", new List<Customer>());
                }
            }
            else if (searchBy == "CustomerName")
            {
                customers = customers.Where(c => c.FullName.Contains(searchValue));
            }

            var result = customers.ToList();
            TempData["SearchResult"] = System.Text.Json.JsonSerializer.Serialize(result);
            return View("Modify", result); // 👈 Render the Modify.cshtml view
        }


        // Populate the User Selected from Searched result
        [HttpPost]
        public IActionResult SelectCustomer(int selectedCustomerId)
        {
            if (TempData["SearchResult"] == null)
            {
                return RedirectToAction("Modify"); // fallback
            }

            var customerList = System.Text.Json.JsonSerializer.Deserialize<List<Customer>>(TempData["SearchResult"].ToString());

            var selectedCustomer = customerList.FirstOrDefault(c => c.CustomerId == selectedCustomerId);
            if (selectedCustomer == null)
            {
                TempData["Error"] = "Selected customer not found.";
                return View("Modify", customerList);
            }

            // Keep showing filtered list
            ViewBag.SelectedCustomer = selectedCustomer;

            // ❗ Store again since TempData clears after use
            TempData["SearchResult"] = System.Text.Json.JsonSerializer.Serialize(customerList);

            return View("Modify", customerList);
        }




                 // Delete LOGIC  
        public IActionResult Delete()
        {
            return View();
        }




                 // Inquire LOGIC  
        public IActionResult Inquire()
        {
            return View();
        }
    }
}
