namespace BuildingStore.Services.Patterns.FactoryMethod
{
    public class RoofingCategoryFactory : CategoryFactory
    {
        public override CategoryProduct CreateProduct() 
        { 
            return new RoofingCategoryProduct();
        }
    }
}
