using BuildingStore.Models;
using System.Reflection;

namespace BuildingStore.Services.Patterns.Bridge
{
    public class ReceiptDocumentBridge : OrderDocumentBridge
    {
        public ReceiptDocumentBridge(IDocumentSenderBridge sender) : base(sender) { }
        public override void GenerateAndSend(Order order, string email)
        {
        }
    }
}
