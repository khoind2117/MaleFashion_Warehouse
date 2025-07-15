using MaleFashion_Warehouse.Server.Common.Enums;

namespace MaleFashion_Warehouse.Server.Models.Entities;

public class ProductVariant
{
    public int Id { get; set; }

    public Size Size { get; set; }

    public int Stock { get; set; }

    public int? ColorId { get; set; }
    public virtual Color? Color { get; set; }

    public int ProductId { get; set; }
    public virtual Product? Product { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
