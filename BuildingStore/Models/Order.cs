using System.ComponentModel.DataAnnotations;

namespace BuildingStore.Models;

public class Order
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime OrderDate { get; set; }

    [Required]
    public string Status { get; set; } = "Processing";

    [Required]
    public string PostOfficeNumber { get; set; } = string.Empty;

    public int UserId { get; set; }
    public User? User { get; set; }
        
    public List<OrderItem> OrderItems { get; set; } = new();
}

