using BuildingStore.Models;

namespace BuildingStore.Services.Patterns.FactoryMethod
{
    public class ElectricalCategoryFactory : CategoryFactory
    {
        public override List<Product> GetProducts(AppDbContext db)
        {
            var products = db.Products.Where(p => p.CategoryId == (byte)EnumCategories.Electrical).ToList();
            foreach (var p in products)
            {
                p.Price *= 1.10m; 
            }

            ApplyBadge(products, "Включає повне страхування");

            return products;
        }
    }
}
