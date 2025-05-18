using System;
using System.Collections.Generic;
using MaleFashion_Warehouse.Server.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MaleFashion_Warehouse.Server.Data;

public partial class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Color> Colors { get; set; }

    public virtual DbSet<Favorite> Favorites { get; set; }

    public virtual DbSet<MainCategory> MainCategories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductVariant> ProductVariants { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<SubCategory> SubCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.ToTable("Cart");

            entity.HasIndex(e => e.UserId, "IX_Cart_UserId")
                .IsUnique()
                .HasFilter("([UserId] IS NOT NULL)");

            entity.HasOne(d => d.User).WithOne(p => p.Cart).HasForeignKey<Cart>(d => d.UserId);
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.ToTable("CartItem");

            entity.HasIndex(e => e.CartId, "IX_CartItem_CartId");

            entity.HasIndex(e => e.ProductVariantId, "IX_CartItem_ProductVariantId");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems).HasForeignKey(d => d.CartId);

            entity.HasOne(d => d.ProductVariant).WithMany(p => p.CartItems).HasForeignKey(d => d.ProductVariantId);
        });

        modelBuilder.Entity<Color>(entity =>
        {
            entity.ToTable("Color");
        });

        modelBuilder.Entity<Favorite>(entity =>
        {
            entity.ToTable("Favorite");

            entity.HasIndex(e => e.ProductVariantId, "IX_Favorite_ProductVariantId");

            entity.HasIndex(e => new { e.UserId, e.ProductVariantId }, "IX_Favorite_UserId_ProductVariantId")
                .IsUnique()
                .HasFilter("([UserId] IS NOT NULL)");

            entity.HasOne(d => d.ProductVariant).WithMany(p => p.Favorites).HasForeignKey(d => d.ProductVariantId);

            entity.HasOne(d => d.User).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<MainCategory>(entity =>
        {
            entity.ToTable("MainCategory");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.ToTable("OrderItem");

            entity.HasIndex(e => e.OrderId, "IX_OrderItem_OrderId");

            entity.HasIndex(e => e.ProductVariantId, "IX_OrderItem_ProductVariantId");

            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems).HasForeignKey(d => d.OrderId);

            entity.HasOne(d => d.ProductVariant).WithMany(p => p.OrderItems).HasForeignKey(d => d.ProductVariantId);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.HasIndex(e => e.SubCategoryId, "IX_Product_SubCategoryId");

            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.SubCategory).WithMany(p => p.Products)
                .HasForeignKey(d => d.SubCategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<ProductVariant>(entity =>
        {
            entity.ToTable("ProductVariant");

            entity.HasIndex(e => e.ColorId, "IX_ProductVariant_ColorId");

            entity.HasIndex(e => e.ProductId, "IX_ProductVariant_ProductId");

            entity.HasIndex(e => e.SizeId, "IX_ProductVariant_SizeId");

            entity.HasOne(d => d.Color).WithMany(p => p.ProductVariants).HasForeignKey(d => d.ColorId);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductVariants).HasForeignKey(d => d.ProductId);

            entity.HasOne(d => d.Size).WithMany(p => p.ProductVariants).HasForeignKey(d => d.SizeId);
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.ToTable("Size");
        });

        modelBuilder.Entity<SubCategory>(entity =>
        {
            entity.ToTable("SubCategory");

            entity.HasIndex(e => e.MainCategoryId, "IX_SubCategory_MainCategoryId");

            entity.HasOne(d => d.MainCategory).WithMany(p => p.SubCategories)
                .HasForeignKey(d => d.MainCategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
