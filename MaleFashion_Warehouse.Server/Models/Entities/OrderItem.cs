using MaleFashion_Warehouse.Server.Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MaleFashion_Warehouse.Server.Models.Entities;

public partial class OrderItem
{
    public int Id { get; set; }

    public required string ProductName { get; set; }

    public required string ProductColor { get; set; }

    public Size ProductSize { get; set; }

    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal UnitPrice { get; set; }

    public Currency Currency { get; set; }

    public int? ProductVariantId { get; set; }
    public virtual ProductVariant? ProductVariant { get; set; }

    public int OrderId { get; set; }
    public virtual Order? Order { get; set; }

}
