using BuildingStore.Models;

namespace BuildingStore.Services.Patterns.Observer
{
    public abstract class OrderSubject
    {
        private List<OrderObserver> observers = new List<OrderObserver>();

        public void Attach(OrderObserver observer)
        {
            observers.Add(observer);
        }
        public void Detach(OrderObserver observer)
        {
            observers.Remove(observer);
        }
        public void Notify(Order order)
        {
            foreach (OrderObserver o in observers)
            {
                o.Update(order);
            }
        }
    }
}