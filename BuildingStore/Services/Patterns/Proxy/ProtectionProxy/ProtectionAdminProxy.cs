using BuildingStore.Models;
using BuildingStore.Services.Patterns.Proxy.ProtectionProxy;

namespace BuildingStore.Services.Patterns.Proxy
{
    public class ProtectionAdminProxy : IAdminLoginService
    {
        private readonly AppDbContext appDbContext;
        private readonly RealAdminLoginService realAdminLoginService;

        public ProtectionAdminProxy(AppDbContext appDbContext, RealAdminLoginService realAdminLoginService)
        {
            this.appDbContext = appDbContext;
            this.realAdminLoginService = realAdminLoginService;
        }

        public bool EnterAdminPanel(string email)
        {
            bool isAdmin = appDbContext.Users.Any(a => a.Email == email && a.Role == UserRole.Admin);

            if (!isAdmin)
            {
                return false;
            }

            return realAdminLoginService.EnterAdminPanel(email); 
        }
    }
}