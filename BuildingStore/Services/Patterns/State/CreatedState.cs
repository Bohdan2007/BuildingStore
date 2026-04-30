using BuildingStore.Models;

namespace BuildingStore.Services.Patterns.State
{
    public class CreatedState : IOrderItemState
    {
        public void Cancel(OrderItem item) { }
        public void Pay(OrderItem item)
        {
            item.ProductStatus = ProductStatus.Processing;
        }
        public void Complete(OrderItem item) => throw new InvalidOperationException("Адмін не може виконати товар, який ще не оплачено.");
    }
}
