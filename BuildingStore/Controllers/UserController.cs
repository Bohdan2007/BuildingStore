using BuildingStore.Services.BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using BuildingStore.Models;
using BuildingStore.Services.Patterns.Bridge;

namespace BuildingStore.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService userService;
        private readonly OrderDocumentBridge documentBridge;

        public UserController(UserService userService, OrderDocumentBridge documentBridge)
        {
            this.userService = userService;
            this.documentBridge = documentBridge;
        }
        
        [HttpGet]
        public IActionResult Profile(string email)
        {
            User user = userService.FindUser(email);

            return View(user);
        }

        [HttpGet]
        public IActionResult AdminProfile(string email)
        {
            User user = userService.FindAdmin(email);

            return View(user);
        }
        [HttpPost]
        public IActionResult CompleteItem(int orderItemId, string adminEmail)
        {
            userService.CompleteOrderItemAdmin(orderItemId);

            return RedirectToAction("AdminProfile", new { email = adminEmail });
        }
    }
}
