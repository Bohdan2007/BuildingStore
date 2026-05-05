using BuildingStore.Models;
using BuildingStore.Services.Patterns.Bridge;

namespace BuildingStore.Services.Patterns.Observer
{
    public class EmailObserver : IOrderObserver
    {
        private readonly OrderDocumentBridge documentGenerator;

        public EmailObserver(OrderDocumentBridge documentGenerator)
        {
            this.documentGenerator = documentGenerator;
        }
        public void OrderChanged(Order order)
        {
            if (order.OrderStatus == OrderStatus.Completed)
            {
                string targetEmail = order.User?.Email;

                Console.WriteLine(targetEmail);
                
                if (!string.IsNullOrEmpty(targetEmail))
                {
                    documentGenerator.GenerateAndSend(order, targetEmail);
                }
            }
        }
    }
}
