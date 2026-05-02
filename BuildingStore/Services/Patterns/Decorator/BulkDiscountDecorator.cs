namespace BuildingStore.Services.Patterns.Decorator
{
    public class BulkDiscountDecorator : AbstractProductDecorator
    {
        private const short BulkPriceThreshold = 5000;
        private const short BulkDiscountValue = 500;

        public BulkDiscountDecorator(IDiscountDecorator component) : base(component) { }
        public override decimal GetPrice()
        {
            decimal price = base.GetPrice();

            if (price > BulkPriceThreshold)
            {
                return price - BulkDiscountValue;
            }
            else
            {
                return price;
            }
        }
    }
}
