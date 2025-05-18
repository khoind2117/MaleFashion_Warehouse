using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace MaleFashion_Warehouse.Server.Models.Entities;

public partial class User : IdentityUser
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Address { get; set; } = null!;

    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }

    public virtual Cart? Cart { get; set; }
    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
