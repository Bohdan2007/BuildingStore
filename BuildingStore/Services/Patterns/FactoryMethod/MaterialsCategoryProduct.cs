using BuildingStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BuildingStore.Services.Patterns.FactoryMethod
{
    public class MaterialsCategoryProduct : CategoryProduct
    {
        public override List<Product> GetProducts(AppDbContext db)
        {
            var products = db.Products.AsNoTracking().Where(p => p.CategoryId == (byte)EnumCategories.Materials).ToList();
            foreach (var p in products)
            {
                p.Price *= 0.70m; 
            }

            ApplyBadge(products, "АКЦІЯ -30%");

            return products;
        }
    }
}
