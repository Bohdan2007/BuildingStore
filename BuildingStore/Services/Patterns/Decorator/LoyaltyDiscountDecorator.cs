namespace BuildingStore.Services.Patterns.Decorator
{
    public class LoyaltyDiscountDecorator : AbstractProductDecorator
    {
        private const decimal LoyaltyDiscountMultiplier = 0.95m;

        public LoyaltyDiscountDecorator(IDiscountDecorator component) : base(component) { }
        public override decimal GetPrice()
        {
            return base.GetPrice() * LoyaltyDiscountMultiplier;
        }
    }
}
