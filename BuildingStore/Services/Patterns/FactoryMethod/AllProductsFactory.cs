using BuildingStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BuildingStore.Services.Patterns.FactoryMethod
{
    public class AllProductsFactory : CategoryFactory
    {
        public override List<Product> GetProducts(AppDbContext db)
        {
            return db.Products.Include(p => p.Category).ToList();
        }
    }
}
