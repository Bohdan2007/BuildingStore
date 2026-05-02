using BuildingStore.Models;
using BuildingStore.Services.Patterns.FactoryMethod;

namespace BuildingStore.Services.Patterns.Proxy.CachingProxy
{
    public interface ICategoryLoaderProxy
    {
        List<Product> LoadProducts(AppDbContext db, EnumCategories category);
    }
}
