using BuildingStore.Models;
using BuildingStore.Services.Patterns.Observer;
using BuildingStore.Services.Patterns.State;
using Microsoft.EntityFrameworkCore;

namespace BuildingStore.Services.BusinessLogic
{
    public class UserService
    {
        private readonly AppDbContext appDbContext;
        private readonly IEnumerable<IOrderObserver> observers;

        public UserService(AppDbContext appDbContext, IEnumerable<IOrderObserver> observers)
        {
            this.appDbContext = appDbContext;
            this.observers = observers;
        }

        public User FindUser(string email)
        {
            var user = appDbContext.Users.Include(u => u.Orders.Where(o => o.OrderStatus == OrderStatus.Completed)).ThenInclude(o => o.OrderItems).ThenInclude(oi => oi.Product).FirstOrDefault(u => u.Email == email);

            return user;
        }
        public User FindAdmin(string email)
        {
            var admin = appDbContext.Users.FirstOrDefault(u => u.Email == email);

            if (admin != null)
            {
                admin.Orders = appDbContext.Orders.Include(o => o.User).Include(o => o.OrderItems.Where(i => i.ProductStatus == ProductStatus.Processing)).ThenInclude(oi => oi.Product).Where(o => (o.AdminId == null || o.AdminId == admin.Id) && o.OrderItems.Any(i => i.ProductStatus == ProductStatus.Processing)).ToList();
            }

            return admin;
        }
        public void RemoveItemFromOrder(int orderItemId)
        {
            var item = appDbContext.OrderItems.Include(i => i.Order).ThenInclude(o => o.OrderItems).FirstOrDefault(i => i.Id == orderItemId);

            if (item != null)
            {
                var state = GetCurrentState(item.ProductStatus);
                state.Cancel(item); 

                appDbContext.OrderItems.Remove(item);
                appDbContext.SaveChanges();

                NotifyObservers(item.Order);
            }
        }
        public void CompleteOrderItemAdmin(int orderItemId)
        {
            var item = appDbContext.OrderItems.Include(i => i.Order).ThenInclude(o => o.User) .Include(i => i.Order).ThenInclude(o => o.OrderItems).ThenInclude(oi => oi.Product) .FirstOrDefault(i => i.Id == orderItemId);

            if (item != null)
            {
                var state = GetCurrentState(item.ProductStatus);

                state.Complete(item);
                appDbContext.SaveChanges();

                NotifyObservers(item.Order);
            }
        }
        private IOrderItemState GetCurrentState(ProductStatus status)
        {
            switch (status)
            {
                case ProductStatus.Created:
                    return new CreatedState();
                case ProductStatus.Processing:
                    return new ProcessingState();
                case ProductStatus.Completed:
                    return new CompletedState();
                default:
                    throw new ArgumentException("Невідомий статус товару");
            }
        }
        private void NotifyObservers(Order order)
        {
            foreach (var observer in observers)
            {
                observer.OrderChanged(order);
            }
        }
    }
}