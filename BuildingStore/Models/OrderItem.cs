using System.ComponentModel.DataAnnotations;

namespace BuildingStore.Models
{
    public enum ProductStatus : byte{ Created, Processing, Completed }
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public ProductStatus ProductStatus { get; set; } = ProductStatus.Created;

        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

    }
}

