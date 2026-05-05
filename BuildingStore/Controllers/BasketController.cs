using BuildingStore.Models;
using BuildingStore.Services.BusinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace BuildingStore.Controllers
{
    public class BasketController : Controller
    {
        private readonly BasketService basketService;

        public BasketController(BasketService basketService)
        {
            this.basketService = basketService;
        }

        [HttpGet]
        public IActionResult Basket()
        {
            string email = basketService.GetUserEmail(User);
            var order = basketService.GetOrCreateBasketOrder(email);
            return View(order);
        }
        [HttpPost]
        public IActionResult AddToBasket(int productId)
        {
            string email = basketService.GetUserEmail(User);
            basketService.AddToBasket(productId, email);

            string referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer))
            {
                return Redirect(referer);
            }

            return RedirectToAction("Products", "Products");
        }
        [HttpPost]
        public IActionResult Increase(int itemId)
        {
            string email = basketService.GetUserEmail(User);
            basketService.ChangeQuantity(itemId, 1, email);
            return RedirectToAction("Basket");
        }
        [HttpPost]
        public IActionResult Decrease(int itemId)
        {
            string email = basketService.GetUserEmail(User);
            basketService.ChangeQuantity(itemId, -1, email);
            return RedirectToAction("Basket");
        }

        [HttpPost]
        public IActionResult Remove(int itemId)
        {
            try
            {
                string email = basketService.GetUserEmail(User);
                basketService.RemoveItem(itemId, email);
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("Basket");
        }

        [HttpGet]
        public IActionResult Payment()
        {
            string email = basketService.GetUserEmail(User);
            var order = basketService.GetOrCreateBasketOrder(email);

            if (order == null || !order.OrderItems.Any())
            {
                return RedirectToAction("Basket");
            }

            return View(order);
        }
        [HttpPost]
        public IActionResult Payment(string email, string postOfficeNumber, string cardNumber, string expiryDate, string cvv)
        {
            string currentUserEmail = basketService.GetUserEmail(User);
            var order = basketService.GetOrCreateBasketOrder(currentUserEmail);

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(postOfficeNumber) ||
                string.IsNullOrWhiteSpace(cardNumber) || string.IsNullOrWhiteSpace(expiryDate) ||
                string.IsNullOrWhiteSpace(cvv))
            {
                ModelState.AddModelError("", "Будь ласка, заповніть усі поля форми оплати.");
                return View(order);
            }

            try
            {
                basketService.ProcessPayment(currentUserEmail, email, postOfficeNumber);
                return RedirectToAction("Success");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(order);
            }
        }

        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }
    }
}