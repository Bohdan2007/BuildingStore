using System.ComponentModel.DataAnnotations;

namespace BuildingStore.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Photo { get; set; } = string.Empty;

        [Required]
        public int Rating { get; set; }

        [Required]
        public int QuantityInStock { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new();

        public List<Review> Reviews { get; set; } = new();

        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}

