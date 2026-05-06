namespace BuildingStore.Services.Patterns.FactoryMethod
{
    public class ElectricalCategoryFactory : CategoryFactory
    {
        public override CategoryProduct CreateProduct() 
        {
            return new ElectricalCategoryProduct();
        }
    }
}
