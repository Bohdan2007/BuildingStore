using BuildingStore.Models;

namespace BuildingStore.Services.Patterns.FactoryMethod
{
    public class ToolsCategoryFactory : CategoryFactory
    {
        public override List<Product> GetProducts(AppDbContext db)
        {
            var products = db.Products.Where(p => p.CategoryId == (byte)EnumCategories.Tools).ToList();

            ApplyBadge(products, "Гарантія 3 роки");

            return products;
        }
    }
}
