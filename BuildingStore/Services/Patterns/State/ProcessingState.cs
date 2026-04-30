using BuildingStore.Models;

namespace BuildingStore.Services.Patterns.State
{
    public class ProcessingState : IOrderItemState
    {
        public void Cancel(OrderItem item) => throw new InvalidOperationException("Товар вже оплачено і комплектується, відмова неможлива.");
        public void Pay(OrderItem item) => throw new InvalidOperationException("Цей товар вже оплачено.");
        public void Complete(OrderItem item)
        {
            item.ProductStatus = ProductStatus.Completed;
        }
    }
}
