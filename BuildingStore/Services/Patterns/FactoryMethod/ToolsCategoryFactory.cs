namespace BuildingStore.Services.Patterns.FactoryMethod
{
    public class ToolsCategoryFactory : CategoryFactory
    {
        public override CategoryProduct CreateProduct() 
        { 
            return new ToolsCategoryProduct();
        }
    }
}
