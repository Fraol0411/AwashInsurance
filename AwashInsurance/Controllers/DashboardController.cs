using Microsoft.AspNetCore.Mvc;

namespace AwashInsurance.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
