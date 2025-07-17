using MaleFashion_Warehouse.Server.Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaleFashion_Warehouse.Server.Models.Entities;

public class Order
{
    public int Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string Address { get; set; }

    public required string PhoneNumber { get; set; }

    public required string Email { get; set; }

    public string? Note { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Total { get; set; }

    public Currency Currency { get; set; }

    public PaymentMethod PaymentMethod { get; set; }
    
    public OrderStatus Status { get; set; }
    
    public DateTimeOffset CreatedDate { get; set; }

    public string? UserId { get; set; }
    public virtual User? User { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
