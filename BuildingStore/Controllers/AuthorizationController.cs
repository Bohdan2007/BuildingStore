using BuildingStore.Services.BusinessLogic;
using BuildingStore.Services.Patterns.Proxy.ProtectionProxy;
using Microsoft.AspNetCore.Mvc;

namespace BuildingStore.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly AuthorizationService authorizationService;
        private readonly IAdminLoginProxy adminLoginService;

        public AuthorizationController(AuthorizationService authorizationService, IAdminLoginProxy adminLoginService)
        {
            this.authorizationService = authorizationService;
            this.adminLoginService = adminLoginService;
        }

        [HttpGet]
        public IActionResult Authorization()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignIn(string email, string password)
        {
            bool isExistUser = authorizationService.SignIn(email, password);
            bool isAdmin = adminLoginService.EnterAdminPanel(email);

            if (isExistUser)
            {
                if (isAdmin)
                {
                    return RedirectToAction("AdminProfile", "User", new { email = email });
                }

                return RedirectToAction("Profile", "User", new {email = email});
            }

            ModelState.AddModelError(string.Empty, "Неправильні дані для входу. Спробуйте ще раз.");
            return View();
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(string name, string email, string password, string passwordVerification)
        {
            bool isExistUser = authorizationService.SignUp(name, email, password, passwordVerification);

            if (isExistUser)
            {
                return RedirectToAction("Profile", "User", new { email = email });
            }

            if (authorizationService.IsSignIn(email))
            {
                ModelState.AddModelError(string.Empty, "Ви вже зареєстровані.");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Неправильні дані для входу. Спробуйте ще раз.");
            }
            
            return View();
        }
    }
}
