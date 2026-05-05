using BuildingStore.Models;

namespace BuildingStore.Services.BusinessLogic
{
    public class AuthorizationService
    {
        private readonly AppDbContext appDbContext;

        public AuthorizationService(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public bool SignIn(string email, string password)
        {
            if (email == null || password == null)
            {
                return false;
            }
            
            var existUser = appDbContext.Users.FirstOrDefault(u => u.Email == email);
            if (existUser == null || existUser.Password != HashPassword(password))
            {
                return false;
            }

            return true;
        }
        public bool SignUp(string name, string email, string password, string passwordVerification)
        {
            if (name == null || email == null || password == null || passwordVerification == null)
            {
                return false;
            }

            var existUser = appDbContext.Users.FirstOrDefault(u => u.Email == email);
            if (existUser != null || password != passwordVerification)
            {
                return false;
            }
            
            User user = new User { Name = name, Email = email, Password = HashPassword(password), Role = UserRole.User };

            appDbContext.Users.Add(user);
            appDbContext.SaveChanges();

            return true;
        }
        public bool IsSignIn(string email)
        {
            return appDbContext.Users.Any(u => u.Email == email);
        }
        private string HashPassword(string password)
        {
            long hashCode = 0L;

            for (int i = 0; i < password.Length; i++)
            {
                hashCode += (password[i] * 31) * i * i;
            }

            byte numberConvert = 16;
            char[] arrayConvert = { 'A', 'v', '@', '1', '~', '&', 'о', 'f', 'м', ')', '-', 'К', '$', 'q', '#', '3' };
            string hashPassword = "";
            byte index = 0;

            for (byte i = 0; i < numberConvert; i++)
            {
                index = (byte)(hashCode % numberConvert);
                hashPassword += arrayConvert[index];
                hashCode = hashCode - ((numberConvert - i) * (numberConvert - i));
            }

            return hashPassword;
        }
    }
}
