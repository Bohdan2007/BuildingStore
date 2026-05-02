using BuildingStore.Models;
using BuildingStore.Services.Patterns.FactoryMethod;

namespace BuildingStore.Services.Patterns.Proxy.CachingProxy
{
    public class CachingCategoryProxy : ICategoryLoaderProxy
    {
        private readonly RealCategoryLoaderProxy realLoader;
        private static readonly Dictionary<EnumCategories, List<Product>> cache = new();
        private const int MaxCacheEntries = 100;

        public CachingCategoryProxy(RealCategoryLoaderProxy realLoader)
        {
            this.realLoader = realLoader;
        }

        public List<Product> LoadProducts(AppDbContext db, EnumCategories category)
        {
            if (cache.ContainsKey(category))
            {
                return cache[category];
            }

            var products = realLoader.LoadProducts(db, category);

            if (cache.Count < MaxCacheEntries)
            {
                cache[category] = products;
            }

            return products;
        }
    }
}
