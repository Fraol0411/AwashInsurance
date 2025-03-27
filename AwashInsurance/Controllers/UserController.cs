using Microsoft.AspNetCore.Mvc;

namespace AwashInsurance.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
