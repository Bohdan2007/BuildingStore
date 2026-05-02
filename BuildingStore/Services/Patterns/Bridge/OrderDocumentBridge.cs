using BuildingStore.Models;

namespace BuildingStore.Services.Patterns.Bridge
{ 
    public abstract class OrderDocumentBridge
    {
        protected IDocumentSenderBridge sender;

        public OrderDocumentBridge(IDocumentSenderBridge sender)
        {
            this.sender = sender;
        }
        public abstract void GenerateAndSend(Order order, string email);
    }  
}
