using System;
using System.Collections.Generic;
using DbmsApp.Controllers;
using Microsoft.EntityFrameworkCore;
using DbmsApp.Models;

namespace DbmsApp.Context;

public partial class PizzaPlaceContext : DbContext
{
    public PizzaPlaceContext()
    {
    }

    public PizzaPlaceContext(DbContextOptions<PizzaPlaceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Coupon> Coupons { get; set; }

    public virtual DbSet<CouponsToOrder> CouponsToOrders { get; set; }
    
    public virtual DbSet<Good> Goods { get; set; }

    public virtual DbSet<GoodsToCoupon> GoodsToCoupons { get; set; }

    public virtual DbSet<GoodsToOrder> GoodsToOrders { get; set; }
    
    public virtual DbSet<ReadableGood> ReadableGoods { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Product> Products { get; set; }


    public virtual DbSet<User> Users { get; set; }
    
    public virtual DbSet<Log> Logs { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=PizzaPlace;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReadableGood>().ToView("ReadableGoods");
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Addresse__3213E83F1BA3A112");

            entity.HasIndex(e => e.UserId, "AddressesByUsers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Adress)
                .HasMaxLength(200)
                .HasColumnName("adress");
            entity.Property(e => e.Entrance)
                .HasMaxLength(10)
                .HasColumnName("entrance");
            entity.Property(e => e.Number)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("number");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Addresses__userI__3B75D760");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Logs__3213E83F7AD02324");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Logg).HasColumnName("log");
        });
        
        modelBuilder.Entity<Coupon>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Coupons__3213E83FBC4871AB");

            entity.HasIndex(e => e.Number, "CouponsByNumber");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateOfExpiration)
                .HasColumnType("date")
                .HasColumnName("dateOfExpiration");
            entity.Property(e => e.DateOfStart)
                .HasColumnType("date")
                .HasColumnName("dateOfStart");
            entity.Property(e => e.Number).HasColumnName("number");
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");
        });

        modelBuilder.Entity<CouponsToOrder>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable(tb => tb.HasTrigger("checkCoupon"));

            entity.HasIndex(e => e.OrderId, "CouponsInOrders");

            entity.Property(e => e.Count)
                .HasDefaultValueSql("((1))")
                .HasColumnName("count");
            entity.Property(e => e.CouponId).HasColumnName("couponId");
            entity.Property(e => e.OrderId).HasColumnName("orderId");

            entity.HasOne(d => d.Coupon).WithMany()
                .HasForeignKey(d => d.CouponId)
                .HasConstraintName("FK__CouponsTo__coupo__534D60F1");

            entity.HasOne(d => d.Order).WithMany()
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__CouponsTo__order__52593CB8");
        });

        modelBuilder.Entity<Good>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3213E83F10E0F018");

            entity.ToTable(tb => tb.HasTrigger("checkSizeOfGood"));

            entity.HasIndex(e => e.ProductId, "GoodsByProduct");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Price)
                .HasDefaultValueSql("((1))")
                .HasColumnType("money")
                .HasColumnName("price");
            entity.Property(e => e.ProductId).HasColumnName("productId");
            entity.Property(e => e.Size)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("size");

            entity.HasOne(d => d.Product).WithMany(p => p.Goods)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Goods__productId__70DDC3D8");
        });

        modelBuilder.Entity<GoodsToCoupon>(entity =>
        {
            entity.HasNoKey();

            entity.HasIndex(e => e.CouponId, "ProductsInCoupons");

            entity.Property(e => e.Count)
                .HasDefaultValueSql("((1))")
                .HasColumnName("count");
            entity.Property(e => e.CouponId).HasColumnName("couponId");
            entity.Property(e => e.ProductId).HasColumnName("productId");

            entity.HasOne(d => d.Coupon).WithMany()
                .HasForeignKey(d => d.CouponId)
                .HasConstraintName("FK__ProductsT__coupo__4AB81AF0");

            entity.HasOne(d => d.Product).WithMany()
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__ProductsT__produ__4BAC3F29");
        });

        modelBuilder.Entity<GoodsToOrder>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable(tb =>
                {
                    tb.HasTrigger("addToCount");
                    tb.HasTrigger("countPrices");
                });

            entity.HasIndex(e => e.OrderId, "ProductsInOrders");

            entity.Property(e => e.Count)
                .HasDefaultValueSql("((1))")
                .HasColumnName("count");
            entity.Property(e => e.OrderId).HasColumnName("orderId");
            entity.Property(e => e.ProductId).HasColumnName("productId");

            entity.HasOne(d => d.Order).WithMany()
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__ProductsT__order__4E88ABD4");

            entity.HasOne(d => d.Product).WithMany()
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__ProductsT__produ__4F7CD00D");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Orders__3213E83FC9FE90E0");

            entity.HasIndex(e => e.UserId, "OrdersByUsers");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AddressId).HasColumnName("addressId");
            entity.Property(e => e.DateOfDelivery)
                .HasColumnType("datetime")
                .HasColumnName("dateOfDelivery");
            entity.Property(e => e.DateOfOrder)
                .HasColumnType("datetime")
                .HasColumnName("dateOfOrder");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Address).WithMany(p => p.Orders)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("FK__Orders__addressI__48CFD27E");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__userId__46E78A0C");
        });
        modelBuilder.Entity<CatalogController.GoodDto>();   
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3213E83F24E2B502");

            entity.HasIndex(e => e.Type, "ProductsByType");

            entity.HasIndex(e => e.Name, "UQ__Products__72E12F1B2B212F7B").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Ingredients)
                .HasMaxLength(100)
                .HasColumnName("ingredients");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsFixedLength()
                .HasColumnName("name");
            entity.Property(e => e.Type)
                .HasMaxLength(14)
                .IsFixedLength()
                .HasColumnName("type");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3213E83FE7FB417F");

            entity.HasIndex(e => e.Email, "UQ__Users__AB6E616411054D34").IsUnique();

            entity.HasIndex(e => e.Phone, "UQ__Users__B43B145F7808159B").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("last_name");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(9)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("phone");
            entity.Property(e => e.Role)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasDefaultValueSql("('usr')")
                .IsFixedLength()
                .HasColumnName("role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
