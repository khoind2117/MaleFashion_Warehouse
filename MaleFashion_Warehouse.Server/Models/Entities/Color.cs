namespace MaleFashion_Warehouse.Server.Models.Entities;

public class Color
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string ColorHex { get; set; }

    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
}
