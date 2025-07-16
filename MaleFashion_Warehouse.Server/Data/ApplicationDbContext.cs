using System;
using System.Collections.Generic;
using MaleFashion_Warehouse.Server.Common.Enums;
using MaleFashion_Warehouse.Server.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MaleFashion_Warehouse.Server.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    #region DbSet
    public DbSet<Cart> Carts { get; set; }

    public DbSet<CartItem> CartItems { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderItem> OrderItems { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<ProductVariant> ProductVariants { get; set; }

    public DbSet<Color> Colors { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        #region FluentAPI
        // Rename Identity tables to remove 'AspNet' prefix and keep naming consistent
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        modelBuilder.Entity<IdentityRole>().ToTable("Roles");
        modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");

        // User
        modelBuilder.Entity<User>(entity =>
        {
            // One-to-many relationship with Order
            entity.HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Cart
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.ToTable("Cart").HasKey(c => c.Id);

            // One-to-one relationship with User
            entity.HasOne(c => c.User)
                .WithOne(u => u.Cart)
                .HasForeignKey<Cart>(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-many relationship with CartItem
            entity.HasMany(c => c.CartItems)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // CartItem
        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.ToTable("CartItem").HasKey(ci => ci.Id);
        });

        // Order
        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order").HasKey(o => o.Id);

            entity.Property(o => o.Total)
                .HasColumnType("decimal(18,2)");

            // One-to-many relationship with OrderItem
            entity.HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // OrderItem
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.ToTable("OrderItem").HasKey(oi => oi.Id);

            entity.Property(oi => oi.UnitPrice)
                .HasColumnType("decimal(18,2)");
        });
        #endregion

        // Product
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product").HasKey(p => p.Id);

            entity.Property(p => p.PriceVnd)
                .HasColumnType("decimal(18,2)");

            entity.Property(p => p.PriceUsd)
                .HasColumnType("decimal(18,2)");

            // One-to-many relationship with ProductVariant
            entity.HasMany(p => p.ProductVariants)
                .WithOne(pv => pv.Product)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ProductVariant
        modelBuilder.Entity<ProductVariant>(entity =>
        {
            entity.ToTable("ProductVariant").HasKey(pv => pv.Id);

            // One-to-many relationship with CartItem
            entity.HasMany(pv => pv.CartItems)
                .WithOne(ci => ci.ProductVariant)
                .HasForeignKey(ci => ci.ProductVariantId)
                .OnDelete(DeleteBehavior.SetNull);

            // One-to-many relationship with OrderItem
            entity.HasMany(pv => pv.OrderItems)
                .WithOne(oi => oi.ProductVariant)
                .HasForeignKey(oi => oi.ProductVariantId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Color
        modelBuilder.Entity<Color>(entity =>
        {
            entity.ToTable("Color").HasKey(c => c.Id);

            // One-to-many relationship with ProductVariant
            entity.HasMany(c => c.ProductVariants)
                .WithOne(pv => pv.Color)
                .HasForeignKey(pv => pv.ColorId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
