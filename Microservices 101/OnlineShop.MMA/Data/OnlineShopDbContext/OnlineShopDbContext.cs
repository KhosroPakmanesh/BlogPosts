using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OnlineShop.MMA.Data.OnlineShopDbContext;

public partial class OnlineShopDbContext : DbContext
{
    public OnlineShopDbContext()
    {
    }

    public OnlineShopDbContext(DbContextOptions<OnlineShopDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<OrderLog> OrderLogs { get; set; }

    public virtual DbSet<PaymentLog> PaymentLogs { get; set; }

    public virtual DbSet<PersonalInfo> PersonalInfos { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductType> ProductTypes { get; set; }

    public virtual DbSet<Shipping> Shippings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=127.0.0.1;Initial Catalog=OnlineShop.MMA.SharedDb;TrustServerCertificate=True;User Id=sa;Password=123456789ABC;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });


        modelBuilder.Entity<UserExtraInfo>(entity =>
        {
            entity.HasKey(e => e.IdUserExtraInfo);

            entity.Property(e => e.FirstName)
            .HasMaxLength(100);

            entity.Property(e => e.LastName)
                .HasMaxLength(100);

            entity.Property(e => e.MobileNumber)
                .HasMaxLength(50);

            entity.Property(e => e.Address)
                .HasMaxLength(500);
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.IdCart);

            entity.ToTable("Carts", "cart");

            entity.Property(e => e.BuyerId).HasMaxLength(450);
            entity.Property(e => e.DiscountVoucher).HasMaxLength(100);

            entity.HasOne(d => d.Buyer).WithMany(p => p.Carts)
                .HasForeignKey(d => d.BuyerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Carts_AspNetUsers");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.IdCartItem);

            entity.ToTable("CartItems", "cart");

            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItems_Carts");

            entity.HasOne(d => d.Product).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartItems_Products");
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.IdDiscount);

            entity.ToTable("Discounts", "discount");

            entity.Property(e => e.BuyerId).HasMaxLength(450);
            entity.Property(e => e.Voucher).HasMaxLength(100);

            entity.HasOne(d => d.Buyer).WithMany(p => p.Discounts)
                .HasForeignKey(d => d.BuyerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Discounts_AspNetUsers");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.IdInventory);

            entity.ToTable("Inventories", "inventory");

            entity.HasOne(d => d.Product).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventories_Products");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.IdOrder);

            entity.ToTable("Orders", "order");

            entity.Property(e => e.BuyerId).HasMaxLength(450);
            entity.Property(e => e.OrderDateTime).HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.IdOrderItem);

            entity.ToTable("OrderItems", "order");

            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItems_Orders");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderItems_Products");
        });

        modelBuilder.Entity<OrderLog>(entity =>
        {
            entity.HasKey(e => e.IdOrderLog);

            entity.ToTable("OrderLogs", "order");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderLogs)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderLogs_Orders");
        });

        modelBuilder.Entity<PaymentLog>(entity =>
        {
            entity.HasKey(e => e.IdPayment);

            entity.ToTable("PaymentLogs", "payment");

            entity.Property(e => e.BandAccountNumber).HasMaxLength(50);
            entity.Property(e => e.BuyerId).HasMaxLength(450);
            entity.Property(e => e.PaymentDateTime).HasColumnType("smalldatetime");
            entity.Property(e => e.PaymentValue).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Buyer).WithMany(p => p.PaymentLogs)
                .HasForeignKey(d => d.BuyerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PaymentLogs_AspNetUsers");
        });

        modelBuilder.Entity<PersonalInfo>(entity =>
        {
            entity.HasKey(e => e.IdPersonalInfo);

            entity.ToTable("PersonalInfos", "identity");

            entity.Property(e => e.Address).HasMaxLength(1000);
            entity.Property(e => e.BankAccountNumber).HasMaxLength(100);
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.User).WithMany(p => p.PersonalInfos)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PersonalInfos_AspNetUsers");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdProduct);

            entity.ToTable("Products", "product");

            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.ProductType).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_ProductTypes");
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.HasKey(e => e.IdProductType);

            entity.ToTable("ProductTypes", "product");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Shipping>(entity =>
        {
            entity.HasKey(e => e.IdShipping);

            entity.ToTable("Shippings", "shipping");

            entity.Property(e => e.ShippingAddress).HasMaxLength(1000);

            entity.HasOne(d => d.Order).WithMany(p => p.Shippings)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Shippings_Orders");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
