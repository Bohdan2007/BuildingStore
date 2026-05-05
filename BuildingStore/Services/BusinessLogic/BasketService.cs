using BuildingStore.Models;
using BuildingStore.Services.Patterns.Decorator;
using BuildingStore.Services.Patterns.State;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BuildingStore.Services.BusinessLogic
{
    public class BasketService
    {
        private readonly AppDbContext appDbContext;
        private readonly ProductService productService;
        private static int lastAdminIndex = 0; 

        public BasketService(AppDbContext context, ProductService productService)
        {
            appDbContext = context;
            this.productService = productService;
        }

        private IOrderItemState GetState(ProductStatus status)
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
                    throw new Exception("Невідомий стан");
            }       
        }
        public string GetUserEmail(ClaimsPrincipal userPrincipal)
        {
            if (userPrincipal.Identity != null && userPrincipal.Identity.IsAuthenticated)
            {
                return userPrincipal.Identity.Name;
            }
            return appDbContext.Users.FirstOrDefault()?.Email ?? "guest@test.com";
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
                order = new Order
                {
                    UserId = user.Id,
                    OrderStatus = OrderStatus.Processing,
                    OrderDate = DateTime.UtcNow,
                    TotalAmount = 0,
                    PostOfficeNumber = "Чернетка"
                };
                appDbContext.Orders.Add(order);
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
                var state = GetState(item.ProductStatus);
                state.Cancel(item); 

                int orderId = item.OrderId;
                appDbContext.OrderItems.Remove(item);
                appDbContext.SaveChanges();
                RecalculateTotal(orderId, userEmail);
            }
        }
        public void RecalculateTotal(int orderId, string userEmail)
        {
            var order = appDbContext.Orders.Include(o => o.OrderItems).FirstOrDefault(o => o.Id == orderId);
            var user = appDbContext.Users.FirstOrDefault(u => u.Email == userEmail);
            if (order == null) return;

            decimal baseSum = order.OrderItems.Sum(oi => oi.Price * oi.Quantity);
            if (baseSum == 0) { order.TotalAmount = 0; appDbContext.SaveChanges(); return; }

            Product summaryProduct = new Product { Price = baseSum };
            IDiscountDecorator processor = new ProductBaseDecorator(summaryProduct);

            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday) 
            {
                processor = new HolidayDiscountDecorator(processor);
            }


            if (user != null && user.Role == UserRole.Admin) 
            {
                processor = new LoyaltyDiscountDecorator(processor);
            }

            processor = new BulkDiscountDecorator(processor);

            order.TotalAmount = processor.GetPrice();
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
                var state = GetState(item.ProductStatus);
                state.Pay(item);
            }

            appDbContext.SaveChanges();
        }
    }
}