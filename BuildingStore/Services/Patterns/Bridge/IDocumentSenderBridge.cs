namespace BuildingStore.Services.Patterns.Bridge
{
    public interface IDocumentSenderBridge
    {
        void Deliver(string docContent, string recipientEmail);
    }
}
