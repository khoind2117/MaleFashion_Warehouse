using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace MaleFashion_Warehouse.Server.Models.Entities;

public class User : IdentityUser
{
    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string Address { get; set; }

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    public virtual Cart? Cart { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
