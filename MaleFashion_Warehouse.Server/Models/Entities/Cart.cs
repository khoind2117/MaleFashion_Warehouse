namespace MaleFashion_Warehouse.Server.Models.Entities;

public class Cart
{
    public int Id { get; set; }
    public Guid? BasketId { get; set; }
    public DateTimeOffset LastUpdated { get; set; }
    
    public string? UserId { get; set; }
    public virtual User? User { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}
