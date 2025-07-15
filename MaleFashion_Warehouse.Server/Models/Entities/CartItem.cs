namespace MaleFashion_Warehouse.Server.Models.Entities;

public class CartItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }

    public int? ProductVariantId { get; set; }
    public virtual ProductVariant? ProductVariant { get; set; }

    public int CartId { get; set; }
    public virtual Cart? Cart { get; set; }
}
