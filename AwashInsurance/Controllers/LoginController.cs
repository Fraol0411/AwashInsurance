using Microsoft.AspNetCore.Mvc;

namespace AwashInsurance.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
