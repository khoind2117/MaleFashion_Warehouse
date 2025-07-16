using MaleFashion_Warehouse.Server.Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaleFashion_Warehouse.Server.Models.Entities;

public class Product
{
    public int Id { get; set; }

    public required string Name { get; set; }
    
    public string? Slug { get; set; }
    
    public Category Category {  get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? PriceVnd { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? PriceUsd { get; set; }

    public string? Description { get; set; }

    public DateTimeOffset CreatedDate { get; set; }

    public DateTimeOffset UpdatedDate { get; set; }

    public ProductStatus Status { get; set; }
    
    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
}
