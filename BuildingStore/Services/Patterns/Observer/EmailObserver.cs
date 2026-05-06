using BuildingStore.Models;
using BuildingStore.Services.Patterns.Bridge;

namespace BuildingStore.Services.Patterns.Observer
{
    public class EmailObserver : OrderObserver
    {
        private readonly OrderDocumentBridge documentGenerator;

        public EmailObserver(OrderDocumentBridge documentGenerator) 
        {
            this.documentGenerator = documentGenerator;
        }

        public override void Update(Order order)
        {
            if (order.OrderStatus == OrderStatus.Completed)
            {
                string targetEmail = order.User?.Email;
                if (!string.IsNullOrEmpty(targetEmail))
                {
                    documentGenerator.GenerateAndSend(order, targetEmail);
                }
            }
        }
    }
}