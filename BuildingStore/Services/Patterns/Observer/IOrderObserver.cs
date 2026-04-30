using BuildingStore.Models;

namespace BuildingStore.Services.Patterns.Observer
{
    public interface IOrderObserver
    {
        void OrderChanged(Order order);
    }
}
