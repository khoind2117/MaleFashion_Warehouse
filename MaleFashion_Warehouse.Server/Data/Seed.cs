using MaleFashion_Warehouse.Server.Models.Entities;
using MaleFashion_Warehouse.Server.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace MaleFashion_Warehouse.Server.Data
{
    public class Seed
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public Seed(ApplicationDbContext context,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedApplicationDbContextAsync()
        {
            #region Seed Roles
            // Check if roles exist; create "Admin" and "User" roles if missing
            var roles = new[] { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    var newRole = new IdentityRole
                    {
                        Name = role,
                        NormalizedName = role.ToUpper(),
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                    };

                    var result = await _roleManager.CreateAsync(newRole);

                    if (result.Succeeded)
                    {
                        Console.WriteLine($"[Seed] Created role: {newRole.Name}, ConcurrencyStamp: {newRole.ConcurrencyStamp}");
                    }
                    else
                    {
                        Console.WriteLine($"[Seed] Failed to create role: {role}");
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine($"  - {error.Code}: {error.Description}");
                        }
                    }
                }
            }
            #endregion

            #region Seed Admin User
            // Check if the admin user exists; create it with default credentials if not
            var adminEmail = "admin@gmail.com";
            var adminUser = await _userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var admin = new User
                {
                    UserName = "admin",
                    FirstName = "Hakuren",
                    LastName = "Admin",
                    Address = "",
                    Email = adminEmail,
                    EmailConfirmed = true,
                    NormalizedUserName = "ADMIN",
                    NormalizedEmail = "ADMIN@GMAIL.COM",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                };

                var result = await _userManager.CreateAsync(admin, "Admin@123");
                if (!result.Succeeded)
                {
                    var errorMessage = string.Join("; ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to create admin user: {errorMessage}");
                }

                await _userManager.AddToRoleAsync(admin, "Admin");
            }
            #endregion
        }
    }
}
