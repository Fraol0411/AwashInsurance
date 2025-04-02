using Microsoft.AspNetCore.Mvc;

namespace AwashInsurance.Controllers
{
    public class UserController : Controller
    {
        public IActionResult UserIdentification()
        {
            return View();
        }

        public IActionResult Lockuser()
        {
            return View();
        }

    }
}
