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




        //public IActionResult Modify()
        //{
        //    return View();
        //}

        public IActionResult Modify(string? searchBy, string? searchValue)
        {
            if (string.IsNullOrWhiteSpace(searchBy) || string.IsNullOrWhiteSpace(searchValue))
            {
                return View(new List<Customer>());
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
                    return View(new List<Customer>());
                }
            }
            else if (searchBy == "CustomerName") // ✅ FIXED this line
            {
                customers = customers.Where(c => c.FullName.Contains(searchValue));
            }

            // ❗ Store filtered results in TempData
            TempData["SearchResult"] = System.Text.Json.JsonSerializer.Serialize(customers.ToList());

            return View(customers.ToList());
        }


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





        public IActionResult Delete()
        {
            return View();
        }
        




        public IActionResult Inquire()
        {
            return View();
        }
    }
}
