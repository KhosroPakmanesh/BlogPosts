using Microsoft.EntityFrameworkCore;
using MVCWebApplication.Entities;



namespace MVCWebApplication.DbContexts;

public partial class OrderDbContext : DbContext
{
    public OrderDbContext()
    {
    }

    public OrderDbContext(DbContextOptions<OrderDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderItem> OrderItems { get; set; }
    public virtual DbSet<Payment> Payments { get; set; }
    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser);
            entity.ToTable("Users", "auth");

            entity.Property(e => e.UserName).HasMaxLength(256);
            entity.Property(e => e.FirstName).HasMaxLength(256);
            entity.Property(e => e.LastName).HasMaxLength(256);
            entity.Property(e => e.Email).HasMaxLength(256);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.IdOrder);
            entity.ToTable("Orders", "order");

            entity.Property(e => e.UserId).HasMaxLength(450);
            entity.Property(e => e.DateTime).HasColumnType("smalldatetime");

            entity.HasOne(a => a.Payment)
                .WithOne(b => b.Order)
                .HasForeignKey<Payment>(e => e.OrderId);
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

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.IdPayment);
            entity.ToTable("Payments", "payment");

            entity.Property(e => e.BankAccountNumber).HasMaxLength(50);
            entity.Property(e => e.DateTime).HasColumnType("smalldatetime");
            entity.Property(e => e.Value).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdProduct);
            entity.ToTable("Products", "product");

            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Seed();
        OnModelCreatingPartial(modelBuilder);
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

