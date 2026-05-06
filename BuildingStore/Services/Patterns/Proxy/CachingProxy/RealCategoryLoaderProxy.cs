using BuildingStore.Models;
using BuildingStore.Services.Patterns.FactoryMethod;

namespace BuildingStore.Services.Patterns.Proxy.CachingProxy
{
    public class RealCategoryLoaderProxy : ICategoryLoaderProxy
    {
        public List<Product> LoadProducts(AppDbContext db, EnumCategories category)
        {
            var factoryProvider = new CategoryFactoryProvider();
            CategoryFactory factory = factoryProvider.GetFactory((int)category);
            CategoryProduct categoryProcessor = factory.CreateProduct();

            return categoryProcessor.GetProducts(db);
        }
    }
}
