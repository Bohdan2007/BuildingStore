using BuildingStore.Models;

namespace BuildingStore.Services.Patterns.FactoryMethod
{
    public class PlumbingCategoryFactory : CategoryFactory
    {
        public override List<Product> GetProducts(AppDbContext db)
        {
            var products = db.Products.Where(p => p.CategoryId == (byte)EnumCategories.Plumbing).ToList();

            ApplyBadge(products, "+ Інструкція з монтажу у подарунок");

            return products;
        }
    }
}
