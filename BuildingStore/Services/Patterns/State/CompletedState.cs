using BuildingStore.Models;

namespace BuildingStore.Services.Patterns.State
{
    public class CompletedState : IOrderItemState
    {
        public void Cancel(OrderItem item) => throw new InvalidOperationException("Товар вже доставлено.");
        public void Pay(OrderItem item) => throw new InvalidOperationException("Товар вже оплачено.");
        public void Complete(OrderItem item) => throw new InvalidOperationException("Вже виконано.");
    }
}
