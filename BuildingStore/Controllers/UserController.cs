using Microsoft.AspNetCore.Mvc;

namespace BuildingStore.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult AdminProfile()
        {
            return View();
        }
    }
}
