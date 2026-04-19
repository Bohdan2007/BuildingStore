using Microsoft.AspNetCore.Mvc;

namespace BuildingStore.Controllers
{
    public class AuthorizationController : Controller
    {
        public IActionResult Authorization()
        {
            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }
    }
}
