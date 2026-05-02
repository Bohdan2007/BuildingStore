namespace BuildingStore.Services.Patterns.Bridge
{
    public class SmtpEmailSenderBridge : IDocumentSenderBridge
    {
        public void Deliver(string docContent, string recipientEmail)
        {
        }
    }
}
