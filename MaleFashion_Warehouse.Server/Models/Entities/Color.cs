using System;
using System.Collections.Generic;

namespace MaleFashion_Warehouse.Server.Models.Entities;

public partial class Color
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string ColorCode { get; set; } = null!;

    public virtual ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
}
