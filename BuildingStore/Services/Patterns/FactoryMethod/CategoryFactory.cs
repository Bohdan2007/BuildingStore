using BuildingStore.Models;

namespace BuildingStore.Services.Patterns.FactoryMethod
{
    public abstract class CategoryFactory
    {
        public abstract List<Product> GetProducts(AppDbContext db);
        protected void ApplyBadge(List<Product> products, string badge)
        {
            foreach (var p in products)
            {
                p.Name = $"{p.Name} [{badge}]";
            }
        }
    }
}
