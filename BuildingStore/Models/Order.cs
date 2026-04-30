using System.ComponentModel.DataAnnotations;

namespace BuildingStore.Models 
{
    public enum OrderStatus : byte { Processing, Completed }
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Processing;

        [Required]
        public string PostOfficeNumber { get; set; } = string.Empty;

        public int UserId { get; set; }
        public User? User { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new();
    }
}


