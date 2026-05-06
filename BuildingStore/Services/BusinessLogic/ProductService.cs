using BuildingStore.Models;
using BuildingStore.Services.Patterns.FactoryMethod;
using BuildingStore.Services.Patterns.Strategy;
using BuildingStore.Services.Patterns.Proxy.CachingProxy; 
using Microsoft.EntityFrameworkCore;

namespace BuildingStore.Services.BusinessLogic
{
    public class ProductService
    {
        private readonly AppDbContext appDbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ICategoryLoaderProxy categoryProxy; 

        public ProductService(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment)
        {
            this.appDbContext = appDbContext;
            this.webHostEnvironment = webHostEnvironment;
            categoryProxy = new CachingCategoryProxy(new RealCategoryLoaderProxy());
        }

        public List<Category> GetCategories()
        {
            return appDbContext.Categories.OrderBy(c => c.Id).ToList();
        }
        public List<Product> FindProducts(string searchName, int? categoryId)
        {
            EnumCategories categoryEnum;

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                categoryEnum = (EnumCategories)categoryId.Value;
            }
            else
            {
                categoryEnum = EnumCategories.All;
            }

            var baseProducts = categoryProxy.LoadProducts(appDbContext, categoryEnum);
            var searchContext = new ProductSearchContext(searchName, categoryId);

            return searchContext.ExecuteSearch(baseProducts, searchName, categoryId);
        }
        public Product GetProductDetails(int id)
        {
            var product = appDbContext.Products.AsNoTracking().Include(p => p.Reviews).FirstOrDefault(p => p.Id == id);

            if (product == null) 
            {
                return null;
            }

            EnumCategories categoryEnum = (EnumCategories)product.CategoryId;
            var processedProducts = categoryProxy.LoadProducts(appDbContext, categoryEnum);
            var processedProduct = processedProducts.FirstOrDefault(p => p.Id == id);

            if (processedProduct != null)
            {
                product.Price = processedProduct.Price;
                product.Name = processedProduct.Name;
            }
            return product;
        }
        public void AddReview(int productId, string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return;

            appDbContext.Reviews.Add(new Review
            {
                ProductId = productId,
                Text = text
            });
            appDbContext.SaveChanges();
        }
        public void CreateProduct(Product product, IFormFile photo)
        {
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
            string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                photo.CopyTo(fileStream);
            }

            product.Photo = uniqueFileName;
            product.Rating = 0;
            product.QuantityInStock = 1;

            appDbContext.Products.Add(product);
            appDbContext.SaveChanges();

            CachingCategoryProxy.ResetCache();
        }
        public bool IsPriceValid(decimal price)
        {
            return price > 0;
        }
        public bool IsProductNameUnique(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) 
            {
                return true;
            }

            return !appDbContext.Products.Any(p => p.Name.ToLower() == name.ToLower());
        }
        public void DeleteProduct(int id)
        {
            var product = appDbContext.Products.FirstOrDefault(p => p.Id == id);

            if (product != null)
            {
                if (!string.IsNullOrEmpty(product.Photo))
                {
                    string filePath = Path.Combine(webHostEnvironment.WebRootPath, "images", product.Photo);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }

                appDbContext.Products.Remove(product);
                appDbContext.SaveChanges();

                CachingCategoryProxy.ResetCache();
            }
        }
    }
}