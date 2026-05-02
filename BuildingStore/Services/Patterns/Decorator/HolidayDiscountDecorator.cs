namespace BuildingStore.Services.Patterns.Decorator
{
    public class HolidayDiscountDecorator : AbstractProductDecorator
    {
        private const decimal HolidayDiscountMultiplier = 0.9m;

        public HolidayDiscountDecorator(IDiscountDecorator component) : base(component) { }
        public override decimal GetPrice() 
        {
            return base.GetPrice() * HolidayDiscountMultiplier;
        }
    }
}
