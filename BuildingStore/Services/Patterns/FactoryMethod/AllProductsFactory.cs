using BuildingStore.Models;

namespace BuildingStore.Services.Patterns.FactoryMethod
{
    public class AllProductsFactory : CategoryFactory
    {
        public override List<Product> GetProducts(AppDbContext db)
        {
            var factories = new List<CategoryFactory>
            {
                new ToolsCategoryFactory(),
                new MaterialsCategoryFactory(),
                new PlumbingCategoryFactory(),
                new ElectricalCategoryFactory(),
                new RoofingCategoryFactory()
            };

            return factories.SelectMany(f => f.GetProducts(db)).ToList();
        }
    }
}

