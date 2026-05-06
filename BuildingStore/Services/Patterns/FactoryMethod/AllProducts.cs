using BuildingStore.Models;

namespace BuildingStore.Services.Patterns.FactoryMethod
{
    public class AllProducts : CategoryProduct
    {
        public override List<Product> GetProducts(AppDbContext db)
        {
            var factories = new List<CategoryProduct>
            {
                new ToolsCategoryProduct(),
                new MaterialsCategoryProduct(),
                new PlumbingCategoryProduct(),
                new ElectricalCategoryProduct(),
                new RoofingCategoryProduct()
            };

            return factories.SelectMany(f => f.GetProducts(db)).ToList();
        }
    }
}

