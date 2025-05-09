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

        //Get: customer/Add
        public IActionResult Add()
        {
            return View();
        }

        //post: customer/Add
        [HttpPost]
       [ValidateAntiForgeryToken]
       public IActionResult Add(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Customers.Add(customer);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }






        public IActionResult Modify()
        {
            return View();
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
