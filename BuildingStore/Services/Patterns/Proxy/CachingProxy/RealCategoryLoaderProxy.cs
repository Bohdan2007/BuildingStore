using BuildingStore.Models;
using BuildingStore.Services.Patterns.FactoryMethod;

namespace BuildingStore.Services.Patterns.Proxy.CachingProxy
{
    public class RealCategoryLoaderProxy : ICategoryLoaderProxy
    {
        public List<Product> LoadProducts(AppDbContext db, EnumCategories category)
        {
            CategoryFactory factory = null;

            switch (category)
            {
                case EnumCategories.Tools:
                    factory = new ToolsCategoryFactory();
                    break;
                case EnumCategories.Materials:
                    factory = new MaterialsCategoryFactory();
                    break;
                case EnumCategories.Plumbing:
                    factory = new PlumbingCategoryFactory();
                    break;
                case EnumCategories.Electrical:
                    factory = new ElectricalCategoryFactory();
                    break;
                case EnumCategories.Roofing:
                    factory = new RoofingCategoryFactory();
                    break;
                default:
                factory = null;
                    break;
            }

            if (factory != null)
            {
                return factory.GetProducts(db);
            }

            return new List<Product>();
        }
    }
}
