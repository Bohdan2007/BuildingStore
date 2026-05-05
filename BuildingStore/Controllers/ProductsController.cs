using BuildingStore.Models;
using BuildingStore.Services.BusinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace BuildingStore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductService productService;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductsController(ProductService productService, IWebHostEnvironment webHostEnvironment)
        {
            this.productService = productService;
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Products(string searchName, int? categoryId)
        {
            var products = productService.FindProducts(searchName, categoryId);
            var categories = productService.GetCategories();
            var model = (Products: products, Categories: categories, SearchName: searchName, CategoryId: categoryId);

            return View(model);
        }

        [HttpGet]
        public IActionResult Product(int id)
        {
            var product = productService.GetProductDetails(id);
            return product == null ? RedirectToAction("Products") : View(product);
        }

        [HttpPost]
        public IActionResult AddReview(int productId, string text)
        {
            productService.AddReview(productId, text);
            return RedirectToAction("Product", new { id = productId });
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            var model = Tuple.Create(new Product(), productService.GetCategories());
            return View(model);
        }

        [HttpPost]
        public IActionResult AddProduct(Product product, IFormFile photo)
        {
            ModelState.Remove("Photo");
            ModelState.Remove("Category");
            ModelState.Remove("OrderItems");
            ModelState.Remove("Reviews");

            bool hasEmptyFields = string.IsNullOrWhiteSpace(product.Name) || string.IsNullOrWhiteSpace(product.Description) || product.CategoryId <= 0 || photo == null || photo.Length == 0;

            if (hasEmptyFields)
            {
                ModelState.AddModelError("", "Заповніть всі обов'язкові поля та додайте фото.");
            }
            else
            {
                if (!productService.IsProductNameUnique(product.Name))
                {
                    ModelState.AddModelError("Name", "Товар з такою назвою вже існує в базі.");
                }
            }

            if (!productService.IsPriceValid(product.Price))
            {
                ModelState.AddModelError("Price", "Ціна повинна бути більшою за нуль.");
            }

            if (!ModelState.IsValid)
            {
                var model = Tuple.Create(product, productService.GetCategories());
                return View(model);
            }

            productService.CreateProduct(product, photo);

            return RedirectToAction("Products");
        }

        [HttpGet]
        public IActionResult DeleteProduct(string searchName, int? categoryId)
        {
            List<Product> products = new List<Product>();

            if (!string.IsNullOrWhiteSpace(searchName) || (categoryId.HasValue && categoryId > 0))
            {
                products = productService.FindProducts(searchName, categoryId);
            }

            var categories = productService.GetCategories();
            var model = Tuple.Create(products, categories, searchName, categoryId);

            return View(model);
        }
        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            productService.DeleteProduct(id);

            return RedirectToAction("DeleteProduct");
        }
    }
}