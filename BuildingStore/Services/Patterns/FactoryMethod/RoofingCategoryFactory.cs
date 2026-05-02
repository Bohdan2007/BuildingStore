using BuildingStore.Models;

namespace BuildingStore.Services.Patterns.FactoryMethod
{
    public class RoofingCategoryFactory : CategoryFactory
    {
        public override List<Product> GetProducts(AppDbContext db)
        {
            var products = db.Products.Where(p => p.CategoryId == (byte)EnumCategories.Roofing).ToList();

            ApplyBadge(products, "Гуртові ціни від 10 одиниць");

            return products;
        }
    }
}
