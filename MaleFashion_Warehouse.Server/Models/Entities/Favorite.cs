using System;
using System.Collections.Generic;

namespace MaleFashion_Warehouse.Server.Models.Entities;

public partial class Favorite
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public int ProductVariantId { get; set; }

    public virtual ProductVariant ProductVariant { get; set; } = null!;

    public virtual User? User { get; set; }
}
