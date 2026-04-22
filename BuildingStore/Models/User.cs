using System.ComponentModel.DataAnnotations;

namespace BuildingStore.Models
{
    public enum UserRole : byte { User, Admin }
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; } = UserRole.User;

        public List<Order> Orders { get; set; } = new();
    }
}

