using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Intex.Models;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }
    private readonly IConfiguration _configuration;

    public MyDbContext(DbContextOptions<MyDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    
    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Bank> banks { get; set; }

    public virtual DbSet<Card_Type> card_types { get; set; }

    public virtual DbSet<Category> categories { get; set; }

    public virtual DbSet<Customer> customers { get; set; }

    public virtual DbSet<Customer_Recommendation> customer_recommendations { get; set; }

    public virtual DbSet<Entry_Mode> entry_modes { get; set; }

    public virtual DbSet<Line_Item> line_items { get; set; }

    public virtual DbSet<Order> orders { get; set; }

    public virtual DbSet<Product> products { get; set; }

    public virtual DbSet<Product_Recommendation> product_recommendations { get; set; }

    public virtual DbSet<Rating> ratings { get; set; }

    public virtual DbSet<Transaction_Type> transaction_types { get; set; }

    public virtual DbSet<User> users { get; set; }

    

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlite("Data Source=app.db");
        }
    }

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

        modelBuilder.Entity<Bank>(entity =>
        {
            entity.HasKey(e => e.bank_id);

            entity.ToTable("bank");

            entity.Property(e => e.name).HasMaxLength(50);
        });

        modelBuilder.Entity<Card_Type>(entity =>
        {
            entity.HasKey(e => e.card_type_id);

            entity.ToTable("card_type");

            entity.Property(e => e.description).HasMaxLength(50);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.category_id);

            entity.ToTable("category");

            entity.Property(e => e.description).HasMaxLength(50);
        });


        modelBuilder.Entity<Customer_Recommendation>(entity =>
        {
            entity.HasKey(e => e.customer_id);

            entity.ToTable("customer_recommendation");

            entity.Property(e => e.customer_id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Entry_Mode>(entity =>
        {
            entity.HasKey(e => e.entry_mode_id);

            entity.ToTable("entry_mode");

            entity.Property(e => e.description).HasMaxLength(50);
        });

        modelBuilder.Entity<Line_Item>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("line_item");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(o => o.transaction_ID).ValueGeneratedOnAdd();
            entity.HasKey(e => e.transaction_ID);
            entity.ToTable("order");

            entity.Property(e => e.country_of_transaction).HasMaxLength(50);
            entity.Property(e => e.day_of_week).HasMaxLength(50);
            entity.Property(e => e.shipping_address).HasMaxLength(50);
        });

        modelBuilder.Entity<Product>(entity =>
        {

            entity.HasKey(e => e.product_id);
            entity.ToTable("product");

            entity.Property(e => e.description).HasMaxLength(2750);
            entity.Property(e => e.img_link).HasMaxLength(150);
            entity.Property(e => e.name).HasMaxLength(100);
            entity.Property(e => e.primary_color).HasMaxLength(50);
            entity.Property(e => e.secondary_color).HasMaxLength(50);
        });

        modelBuilder.Entity<Product_Recommendation>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("product_recommendation");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.product_ID);

            entity.ToTable("rating");

            entity.Property(e => e.rating1).HasColumnName("rating");
        });

        modelBuilder.Entity<Transaction_Type>(entity =>
        {
            entity.HasKey(e => e.transaction_type_id);

            entity.ToTable("transaction_type");

            entity.Property(e => e.description).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.user_id);
            entity.ToTable("user");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.customer_ID);
            entity.ToTable("customer");
            entity.HasOne(e => e.User)
                .WithOne(u => u.Customer)
                .HasForeignKey<Customer>(c => c.customer_ID);
            entity.Property(e => e.customer_ID).ValueGeneratedNever();
            entity.Property(e => e.country_of_residence).HasMaxLength(50);
            entity.Property(e => e.first_name).HasMaxLength(50);
            entity.Property(e => e.gender).HasMaxLength(50);
            entity.Property(e => e.last_name).HasMaxLength(50);
        });
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
