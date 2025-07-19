using MaleFashion_Warehouse.Server.Common.Enums;
using MaleFashion_Warehouse.Server.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MaleFashion_Warehouse.Server.Data
{
    public class Seed
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly Random _random = new();

        public Seed(ApplicationDbContext context,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
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

            #region Seed default Admin and 5 test Users
            var adminUserName = "admin";
            var adminPassword = "Admin@123";
            var adminEmail = "admin@gmail.com";
            var adminUser = await _userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var admin = new User
                {
                    UserName = adminUserName,
                    FirstName = "Hakuren",
                    LastName = "Admin",
                    Address = "",
                    Email = adminEmail,
                    EmailConfirmed = true,
                    NormalizedUserName = adminUserName.ToUpperInvariant(),
                    NormalizedEmail = adminEmail.ToUpperInvariant(),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                };

                var result = await _userManager.CreateAsync(admin, adminPassword);
                if (!result.Succeeded)
                {
                    var errorMessage = string.Join("; ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to create admin user: {errorMessage}");
                }

                await _userManager.AddToRoleAsync(admin, "Admin");
            }

            var userUserNames = new[] { "user_1", "user_2", "user_3", "user_4", "user_5" };
            var userPassword = "User@123";
            var userEmails = new[] {
                "user_1@gmail.com",
                "user_2@gmail.com",
                "user_3@gmail.com",
                "user_4@gmail.com",
                "user_5@gmail.com"
            };
            var firstNames = new[] { "Alice", "Bob", "Charlie", "Diana", "Ethan" };
            var lastNames = new[] { "Nguyen", "Tran", "Le", "Pham", "Hoang" };
            var addresses = new[] {
                "123 Apple St",
                "456 Banana Rd",
                "789 Cherry Blvd",
                "321 Mango Ave",
                "654 Peach Way"
            };

            for (int i = 0; i < userUserNames.Length; i++)
            {
                var existing = await _userManager.FindByEmailAsync(userEmails[i]);
                if (existing != null) continue;

                var user = new User
                {
                    UserName = userUserNames[i],
                    Email = userEmails[i],
                    FirstName = firstNames[i],
                    LastName = lastNames[i],
                    Address = addresses[i],
                    EmailConfirmed = true,
                    NormalizedUserName = userUserNames[i].ToUpperInvariant(),
                    NormalizedEmail = userEmails[i].ToUpperInvariant(),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                };

                var result = await _userManager.CreateAsync(user, userPassword);
                if (!result.Succeeded)
                {
                    var errorMessage = string.Join("; ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to create user {user.UserName}: {errorMessage}");
                }

                await _userManager.AddToRoleAsync(user, "User");
            }
            #endregion

            #region Seed Color
            var colors = new List<Color>
            {
                new Color { Name = "Black", ColorHex = "#000000" },
                new Color { Name = "White", ColorHex = "#FFFFFF" },
                new Color { Name = "Red", ColorHex = "#FF0000" },
                new Color { Name = "Blue", ColorHex = "#0000FF" },
                new Color { Name = "Green", ColorHex = "#008000" },
                new Color { Name = "Yellow", ColorHex = "#FFFF00" },
                new Color { Name = "Orange", ColorHex = "#FFA500" },
                new Color { Name = "Purple", ColorHex = "#800080" },
                new Color { Name = "Gray", ColorHex = "#808080" },
                new Color { Name = "Brown", ColorHex = "#A52A2A" },
            };

            var existingColorNames = await _context.Colors
                .Where(c => colors.Select(x => x.Name).Contains(c.Name))
                .Select(c => c.Name)
                .ToListAsync();

            var newColors = colors.Where(c => !existingColorNames.Contains(c.Name));

            await _context.Colors.AddRangeAsync(newColors);
            await _context.SaveChangesAsync();
            #endregion

            #region Seed Products + Variants
            var start = new DateTimeOffset(2025, 7, 1, 0, 0, 0, TimeSpan.Zero);
            var end = new DateTimeOffset(2025, 7, 15, 23, 59, 59, TimeSpan.Zero);

            var productList = new List<(string Name, int Category, string Description, decimal Usd, decimal Vnd)>
            {
                ("Men's Classic Shirt", 2, "A premium quality men's shirt, suitable for formal occasions.", 15, 345000),
                ("Classic TShirt", 1, "Comfortable cotton T-shirt for everyday wear.", 20, 460000),
                ("Classic Jacket", 4, "Stylish and warm jacket for colder weather.", 25, 575000),
                ("Slim Fit Jeans", 3, "Durable slim fit jeans for casual looks.", 30, 690000),
                ("Hoodie Zipper", 2, "Warm and cozy hoodie with zipper.", 28, 645000),
                ("Sports Shorts", 1, "Lightweight and breathable shorts for sports.", 18, 414000),
                ("Formal Trousers", 4, "Elegant trousers for business events.", 35, 805000),
                ("Denim Jacket", 4, "Rugged denim jacket with timeless appeal.", 40, 920000),
                ("Polo Shirt", 1, "Classic polo for semi-formal outings.", 22, 506000),
                ("Sweatpants", 3, "Comfortable sweatpants for home and gym.", 26, 598000)
            };

            var existingSlugs = await _context.Products.Select(p => p.Slug).ToListAsync();
            var colorIds = await _context.Colors.Select(c => c.Id).ToListAsync();

            foreach (var productInfo in productList)
            {
                var slug = productInfo.Name.ToLower().Replace(" ", "-");

                if (existingSlugs.Contains(slug)) continue;

                var createdDate = start.AddDays(_random.Next((end - start).Days))
                                       .AddHours(_random.Next(0, 24))
                                       .AddMinutes(_random.Next(0, 60));

                var product = new Product
                {
                    Name = productInfo.Name,
                    Slug = slug,
                    Category = (Category)productInfo.Category,
                    Description = productInfo.Description,
                    PriceUsd = productInfo.Usd,
                    PriceVnd = productInfo.Vnd,
                    CreatedDate = createdDate,
                    UpdatedDate = createdDate.AddMinutes(_random.Next(0, 120)),
                    Status = ProductStatus.Active,
                };

                // Seed 3 variants with different sizes and random colors
                var sizes = new[] { Size.S, Size.M, Size.L };

                foreach (var size in sizes)
                {
                    product.ProductVariants.Add(new ProductVariant
                    {
                        Size = size,
                        Stock = _random.Next(10, 51),
                        ColorId = colorIds[_random.Next(colorIds.Count)],
                    });
                }

                await _context.Products.AddAsync(product);
            }

            await _context.SaveChangesAsync();
            #endregion

            #region Seed Carts and CartItems (3 variants per cart)

            var usernames = new[] { "user_1", "user_2", "user_3", "user_4", "user_5" };

            var usersInDb = await _userManager.Users
                .Where(u => usernames.Contains(u.UserName))
                .ToListAsync();

            var allVariants = await _context.ProductVariants.ToListAsync();

            foreach (var user in usersInDb)
            {
                // Kiểm tra user đã có cart chưa
                var hasCart = await _context.Carts.AnyAsync(c => c.UserId == user.Id);
                if (hasCart) continue;

                var cart = new Cart
                {
                    UserId = user.Id,
                    BasketId = Guid.NewGuid(),
                    LastUpdated = DateTimeOffset.UtcNow,
                };

                // Chọn ngẫu nhiên 3 variant khác nhau
                var selectedVariants = allVariants
                    .OrderBy(_ => Guid.NewGuid())
                    .Take(3)
                    .ToList();

                foreach (var variant in selectedVariants)
                {
                    var cartItem = new CartItem
                    {
                        ProductVariantId = variant.Id,
                        Quantity = _random.Next(1, 5)
                    };

                    cart.CartItems.Add(cartItem);
                }

                await _context.Carts.AddAsync(cart);
            }

            await _context.SaveChangesAsync();
            #endregion

            #region Seed Orders and OrderItems (3 orders with 3 items per order per user)

            var productVariants = await _context.ProductVariants
                .Include(pv => pv.Product)
                .Include(pv => pv.Color)
                .ToListAsync();

            foreach (var user in usersInDb)
            {
                for (int i = 0; i < 3; i++) // 3 orders per user
                {
                    var createdDate = new DateTimeOffset(2025, 7, 16, _random.Next(8, 20), _random.Next(0, 60), 0, TimeSpan.Zero);

                    var order = new Order
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Address = user.Address ?? "Địa chỉ mặc định",
                        PhoneNumber = "0909123456",
                        Email = user.Email!,
                        Note = null,
                        Currency = Currency.VND,
                        PaymentMethod = PaymentMethod.COD,
                        Status = OrderStatus.Pending,
                        CreatedDate = createdDate,
                        UserId = user.Id,
                        Total = 0
                    };

                    var selectedVariants = productVariants
                        .OrderBy(_ => Guid.NewGuid())
                        .Take(3)
                        .ToList();

                    foreach (var variant in selectedVariants)
                    {
                        var quantity = _random.Next(1, 4);
                        var unitPrice = variant.Product?.PriceVnd ?? 100000;

                        var item = new OrderItem
                        {
                            ProductVariantId = variant.Id,
                            Quantity = quantity,
                            UnitPrice = unitPrice,
                            Currency = Currency.VND,
                            ProductName = variant.Product?.Name ?? "Unknown",
                            ProductColor = variant.Color?.Name ?? "Unknown",
                            ProductSize = variant.Size
                        };

                        order.OrderItems.Add(item);
                        order.Total += unitPrice * quantity;
                    }

                    await _context.Orders.AddAsync(order);
                }
            }

            await _context.SaveChangesAsync();

            #endregion
        }
    }
}
