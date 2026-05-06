using BuildingStore.Models;
using BuildingStore.Services.Patterns.FactoryMethod;

namespace BuildingStore.Services.Patterns.Proxy.CachingProxy
{
    public class RealCategoryLoaderProxy : ICategoryLoaderProxy
    {
        public List<Product> LoadProducts(AppDbContext db, EnumCategories category)
        {
            CategoryProduct factory = null;

            switch (category)
            {
                case EnumCategories.Tools:
                    factory = new ToolsCategoryProduct();
                    break;
                case EnumCategories.Materials:
                    factory = new MaterialsCategoryProduct();
                    break;
                case EnumCategories.Plumbing:
                    factory = new PlumbingCategoryProduct();
                    break;
                case EnumCategories.Electrical:
                    factory = new ElectricalCategoryProduct();
                    break;
                case EnumCategories.Roofing:
                    factory = new RoofingCategoryProduct();
                    break;
                default:
                factory = new AllProducts();
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
