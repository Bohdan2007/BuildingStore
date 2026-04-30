using BuildingStore.Services.BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using BuildingStore.Models;

namespace BuildingStore.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService userService;

        public UserController(UserService userService)
        {
            this.userService = userService;
        }
        
        [HttpGet]
        public IActionResult Profile(string email)
        {
            User user = userService.FindUser(email);

            return View();
        }

        [HttpGet]
        public IActionResult AdminProfile(string email)
        {
            User user = userService.FindUser(email);

            return View();
        }
    }
}
