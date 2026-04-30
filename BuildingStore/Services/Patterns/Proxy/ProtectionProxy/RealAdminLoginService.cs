namespace BuildingStore.Services.Patterns.Proxy.ProtectionProxy
{
    public class RealAdminLoginService : IAdminLoginService
    {
        public bool EnterAdminPanel(string email)
        {
            return true;
        }
    }
}
