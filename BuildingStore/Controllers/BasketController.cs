using Microsoft.AspNetCore.Mvc;

namespace BuildingStore.Controllers
{
    public class BasketController : Controller
    {
        public IActionResult Basket()
        {
            return View();
        }

        public IActionResult Payment()
        {
            return View();
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
