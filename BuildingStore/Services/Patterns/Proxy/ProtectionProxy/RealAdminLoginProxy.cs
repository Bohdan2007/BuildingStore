namespace BuildingStore.Services.Patterns.Proxy.ProtectionProxy
{
    public class RealAdminLoginProxy : IAdminLoginProxy
    {
        public bool EnterAdminPanel(string email)
        {
            return true;
        }
    }
}
