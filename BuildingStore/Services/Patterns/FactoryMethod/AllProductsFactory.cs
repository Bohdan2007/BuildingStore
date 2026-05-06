namespace BuildingStore.Services.Patterns.FactoryMethod
{
    public class AllProductsFactory : CategoryFactory
    {
        public override CategoryProduct CreateProduct() 
        {
            return new AllProducts();
        }
    }
}
