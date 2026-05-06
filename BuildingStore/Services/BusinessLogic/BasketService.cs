using BuildingStore.Models;
using BuildingStore.Services.Patterns.Decorator;
using BuildingStore.Services.Patterns.Observer;
using BuildingStore.Services.Patterns.State;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BuildingStore.Services.BusinessLogic
{
    public class BasketService : OrderSubject
    {
        private readonly AppDbContext appDbContext;
        private readonly ProductService productService;
        private static int lastAdminIndex = 0;

        public BasketService(AppDbContext context, ProductService productService, IEnumerable<OrderObserver> observers)
        {
            appDbContext = context;
            this.productService = productService;

            foreach (var observer in observers)
            {
                Attach(observer);
            }
        }

        public string GetUserEmail(ClaimsPrincipal userPrincipal)
        {
            if (userPrincipal.Identity != null && userPrincipal.Identity.IsAuthenticated)
            {
                return userPrincipal.Identity.Name;
            }

            return appDbContext.Users.FirstOrDefault()?.Email;
        }
        public Order GetOrCreateBasketOrder(string userEmail)
        {
            var user = appDbContext.Users.FirstOrDefault(u => u.Email == userEmail);
            
            if (user == null) 
            {
                return null;
            }

            var order = appDbContext.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product).FirstOrDefault(o => o.UserId == user.Id && o.OrderStatus == OrderStatus.Processing && o.AdminId == null);

            if (order == null)
            {
                appDbContext.Orders.Add(new Order
                {
                    UserId = user.Id,
                    OrderStatus = OrderStatus.Processing,
                    OrderDate = DateTime.UtcNow,
                    TotalAmount = 0,
                    PostOfficeNumber = "Чернетка"
                });
                appDbContext.SaveChanges();
            }

            return order;
        }
        public void AddToBasket(int productId, string userEmail)
        {
            var order = GetOrCreateBasketOrder(userEmail);
            if (order == null) 
            {
                return;
            }

            var productWithFactoryPrice = productService.GetProductDetails(productId);
            if (productWithFactoryPrice == null) 
            {
                return;
            }

            var existingItem = appDbContext.OrderItems.FirstOrDefault(oi => oi.OrderId == order.Id && oi.ProductId == productId);
            if (existingItem != null) 
            {
                existingItem.Quantity++;
            }
            else
            {
                appDbContext.OrderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = productId,
                    Quantity = 1,
                    Price = productWithFactoryPrice.Price,
                    ProductStatus = ProductStatus.Created
                });
            }

            appDbContext.SaveChanges();
            RecalculateTotal(order.Id, userEmail);
        }
        public void ChangeQuantity(int itemId, int delta, string userEmail)
        {
            var item = appDbContext.OrderItems.FirstOrDefault(oi => oi.Id == itemId);
            if (item != null)
            {
                item.Quantity += delta;
                if (item.Quantity <= 0) appDbContext.OrderItems.Remove(item);

                appDbContext.SaveChanges();
                RecalculateTotal(item.OrderId, userEmail);
            }
        }
        public void RemoveItem(int itemId, string userEmail)
        {
            var item = appDbContext.OrderItems.FirstOrDefault(oi => oi.Id == itemId);
            if (item != null)
            {
                var itemContext = new OrderItemContext(item);
                if (itemContext.Cancel())
                {
                    int orderId = item.OrderId;
                    appDbContext.OrderItems.Remove(item);
                    appDbContext.SaveChanges();
                    RecalculateTotal(orderId, userEmail);
                }
            }
        }
        public void RecalculateTotal(int orderId, string userEmail)
        {
            var order = appDbContext.Orders.Include(o => o.OrderItems).FirstOrDefault(o => o.Id == orderId);
            var user = appDbContext.Users.FirstOrDefault(u => u.Email == userEmail);
            if (order == null) 
            {
                return;
            }

            decimal baseSum = order.OrderItems.Sum(oi => oi.Price * oi.Quantity);
            var calculator = new DiscountCalculator();
            order.TotalAmount = calculator.CalculateFinalPrice(baseSum, user);

            appDbContext.SaveChanges();
        }
        public void ProcessPayment(string currentUserEmail, string formEmail, string postOfficeNumber)
        {
            var customer = appDbContext.Users.FirstOrDefault(u => u.Email == formEmail);
            if (customer == null) 
            {
                throw new Exception("Помилка: Користувача з таким Email не знайдено.");
            }

            var order = GetOrCreateBasketOrder(currentUserEmail);
            if (order == null || !order.OrderItems.Any()) 
            {
                throw new Exception("Ваш кошик порожній.");
            }

            order.UserId = customer.Id;

            var admins = appDbContext.Users.Where(u => u.Role == UserRole.Admin).OrderBy(u => u.Id).ToList();
            if (admins.Any())
            {
                order.AdminId = admins[lastAdminIndex % admins.Count].Id;
                lastAdminIndex++;
            }

            order.OrderStatus = OrderStatus.Processing;
            order.PostOfficeNumber = postOfficeNumber;
            order.OrderDate = DateTime.UtcNow;

            foreach (var item in order.OrderItems)
            {
                var itemContext = new OrderItemContext(item);
                itemContext.Pay();
            }

            appDbContext.SaveChanges();
            Notify(order);
        }
    }
}