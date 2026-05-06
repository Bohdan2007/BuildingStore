namespace BuildingStore.Services.Patterns.FactoryMethod
{
    public class MaterialsCategoryFactory : CategoryFactory
    {
        public override CategoryProduct CreateProduct() 
        { 
            return new MaterialsCategoryProduct();
        }
    }
}
