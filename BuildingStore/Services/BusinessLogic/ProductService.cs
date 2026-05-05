using BuildingStore.Models;
using BuildingStore.Services.Patterns.FactoryMethod;
using BuildingStore.Services.Patterns.Strategy;
using Microsoft.EntityFrameworkCore;

namespace BuildingStore.Services.BusinessLogic
{
    public class ProductService
    {
        private readonly AppDbContext appDbContext;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductService(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment)
        {
            this.appDbContext = appDbContext;
            this.webHostEnvironment = webHostEnvironment;
        }

        public List<Category> GetCategories()
        {
            return appDbContext.Categories.OrderBy(c => c.Id).ToList();
        }
        public List<Product> FindProducts(string searchName, int? categoryId)
        {
            CategoryFactory factory = GetCategoryFactory(categoryId);

            var baseProducts = factory.GetProducts(appDbContext);
            var searchContext = new ProductSearchContext();
            bool hasName = !string.IsNullOrWhiteSpace(searchName);
            bool hasCategory = categoryId.HasValue && categoryId > 0;

            if (hasName && hasCategory)
            {
                searchContext.SetStrategy(new CombinedSearchStrategy());
            }
            else if (hasName)
            {
                searchContext.SetStrategy(new SearchByNameStrategy());
            }
            else if (hasCategory)
            {
                searchContext.SetStrategy(new SearchByCategoryStrategy());
            }
            else
            {
                searchContext.SetStrategy(new DefaultSearchStrategy());
            }

            return searchContext.ExecuteSearch(baseProducts, searchName, categoryId);
        }
        private CategoryFactory GetCategoryFactory(int? categoryId)
        {
            switch (categoryId)
            {
                case (byte)EnumCategories.Tools:
                    return new ToolsCategoryFactory();
                case (byte)EnumCategories.Materials:
                    return new MaterialsCategoryFactory();
                case (byte)EnumCategories.Plumbing:
                    return new PlumbingCategoryFactory();
                case (byte)EnumCategories.Electrical:
                    return new ElectricalCategoryFactory();
                case (byte)EnumCategories.Roofing:
                    return new RoofingCategoryFactory();
                default:
                    return new AllProductsFactory();
            }
        }
        public Product GetProductDetails(int id)
        {
            var product = appDbContext.Products.AsNoTracking().Include(p => p.Reviews).FirstOrDefault(p => p.Id == id);

            if (product == null) return null;

            var factory = GetCategoryFactory(product.CategoryId);
            var processedProduct = factory.GetProducts(appDbContext).FirstOrDefault(p => p.Id == id);

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
        }
        public bool IsPriceValid(decimal price)
        {
            return price > 0;
        }
        public bool IsProductNameUnique(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return true;
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
            }
        }
    }
}