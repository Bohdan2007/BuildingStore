using BuildingStore.Models;

namespace BuildingStore.Services.Patterns.Observer
{
    public abstract class OrderObserver
    {
        public abstract void Update(Order order);
    }
}