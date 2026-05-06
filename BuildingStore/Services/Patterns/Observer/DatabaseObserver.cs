using BuildingStore.Models;

namespace BuildingStore.Services.Patterns.Observer
{
    public class DatabaseObserver : OrderObserver
    {
        private readonly AppDbContext appDbContext;

        public DatabaseObserver(AppDbContext appDbContext) 
        {
            this.appDbContext = appDbContext;
        }

        public override void Update(Order order)
        {
            bool allItemsCompleted = order.OrderItems.Any() && order.OrderItems.All(i => i.ProductStatus == ProductStatus.Completed);

            if (allItemsCompleted && order.OrderStatus != OrderStatus.Completed)
            {
                order.OrderStatus = OrderStatus.Completed;
                appDbContext.Orders.Update(order);
                appDbContext.SaveChanges();
            }
        }
    }
}