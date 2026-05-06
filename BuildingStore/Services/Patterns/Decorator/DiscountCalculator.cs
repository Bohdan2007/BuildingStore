using BuildingStore.Models;
using System;

namespace BuildingStore.Services.Patterns.Decorator
{
    public class DiscountCalculator
    {
        public decimal CalculateFinalPrice(decimal baseSum, User user)
        {
            if (baseSum == 0) 
            {
                return 0;
            }

            Product summaryProduct = new Product { Price = baseSum };
            IDiscountDecorator processor = new ProductBaseDecorator(summaryProduct);

            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                processor = new HolidayDiscountDecorator(processor);
            }

            if (user != null && user.Role == UserRole.Admin)
            {
                processor = new LoyaltyDiscountDecorator(processor);
            }

            processor = new BulkDiscountDecorator(processor);

            return processor.GetPrice();
        }
    }
}