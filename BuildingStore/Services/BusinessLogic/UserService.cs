using BuildingStore.Models;
using BuildingStore.Services.Patterns.Observer;
using BuildingStore.Services.Patterns.State;
using Microsoft.EntityFrameworkCore;

namespace BuildingStore.Services.BusinessLogic
{
    public class UserService : OrderSubject
    {
        private readonly AppDbContext appDbContext;

        public UserService(AppDbContext appDbContext, IEnumerable<OrderObserver> observers)
        {
            this.appDbContext = appDbContext;

            foreach (var observer in observers)
            {
                Attach(observer);
            }
        }

        public User FindUser(string email) 
        {
            return appDbContext.Users.Include(u => u.Orders.Where(o => o.OrderStatus == OrderStatus.Completed)).ThenInclude(o => o.OrderItems).ThenInclude(oi => oi.Product).FirstOrDefault(u => u.Email == email);
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
                var itemContext = new OrderItemContext(item);

                if (itemContext.Cancel())
                {
                    var order = item.Order;
                    appDbContext.OrderItems.Remove(item);
                    appDbContext.SaveChanges();
                    Notify(order);
                }
            }
        }
        public void CompleteOrderItemAdmin(int orderItemId)
        {
            var item = appDbContext.OrderItems.Include(i => i.Order).ThenInclude(o => o.User).Include(i => i.Order).ThenInclude(o => o.OrderItems).ThenInclude(oi => oi.Product).FirstOrDefault(i => i.Id == orderItemId);

            if (item != null)
            {
                var itemContext = new OrderItemContext(item);

                if (itemContext.Complete())
                {
                    appDbContext.SaveChanges();
                    Notify(item.Order);
                }
            }
        }
    }
}