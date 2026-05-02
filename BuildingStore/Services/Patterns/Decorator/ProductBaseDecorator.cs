using BuildingStore.Models;

namespace BuildingStore.Services.Patterns.Decorator
{
    public class ProductBaseDecorator : IDiscountDecorator
    {
        private readonly Product product;

        public ProductBaseDecorator(Product product)
        {
            this.product = product;
        }
        public decimal GetPrice()
        {
            return product.Price;
        }
    }
}
