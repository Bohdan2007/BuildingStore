using BuildingStore.Models;

namespace BuildingStore.Services.Patterns.State
{
    public interface IOrderItemState
    {
        void Cancel(OrderItem item);
        void Pay(OrderItem item);
        void Complete(OrderItem item);  
    }
}
