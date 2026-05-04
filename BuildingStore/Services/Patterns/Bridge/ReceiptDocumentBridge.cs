using BuildingStore.Models;
using System.Text;

namespace BuildingStore.Services.Patterns.Bridge
{
    public class ReceiptDocumentBridge : OrderDocumentBridge
    {
        public ReceiptDocumentBridge(IDocumentSenderBridge sender) : base(sender) { }

        public override void GenerateAndSend(Order order, string email)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Дякуємо за покупку в Z-Build!");
            sb.AppendLine($"Замовлення №: {order.Id}");
            sb.AppendLine($"Дата оформлення: {order.OrderDate:dd.MM.yyyy HH:mm}");
            sb.AppendLine($"Доставка: {order.PostOfficeNumber}");
            sb.AppendLine("--------------------------------------------------");
            sb.AppendLine("Ваші товари:");

            if (order.OrderItems != null && order.OrderItems.Count > 0)
            {
                foreach (var item in order.OrderItems)
                {
                    var name = item.Product?.Name ?? "Товар";
                    var price = item.Product?.Price ?? 0;
                    sb.AppendLine($"- {name} — {item.Quantity} шт. x {price:N2} ₴");
                }
            }

            sb.AppendLine("--------------------------------------------------");
            sb.AppendLine($"Загальна сума: {order.TotalAmount:N2} ₴");

            sender.Deliver(sb.ToString(), email);
        }
    }
}
