using System;
using System.Collections.Generic;

namespace MaleFashion_Warehouse.Server.Models.Entities;

public partial class SubCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public int? MainCategoryId { get; set; }

    public virtual MainCategory? MainCategory { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
