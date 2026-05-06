namespace BuildingStore.Services.Patterns.FactoryMethod
{
    public class PlumbingCategoryFactory : CategoryFactory
    {
        public override CategoryProduct CreateProduct() 
        {
            return new PlumbingCategoryProduct();
        }
    }
}
