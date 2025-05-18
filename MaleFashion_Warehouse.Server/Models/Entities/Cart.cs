using System;
using System.Collections.Generic;

namespace MaleFashion_Warehouse.Server.Models.Entities;

public partial class Cart
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public Guid? BasketId { get; set; }

    public DateTime? LastUpdated { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual User? User { get; set; }
}
