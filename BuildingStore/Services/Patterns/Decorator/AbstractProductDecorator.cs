namespace BuildingStore.Services.Patterns.Decorator
{
    public abstract class AbstractProductDecorator : IDiscountDecorator
    {
        protected IDiscountDecorator component;

        public AbstractProductDecorator(IDiscountDecorator component)
        {
            this.component = component;
        }
        public virtual decimal GetPrice() 
        {
            return component.GetPrice();
        }
    }
}
