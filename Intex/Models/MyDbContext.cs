using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Intex.Models;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<bank> banks { get; set; }

    public virtual DbSet<card_type> card_types { get; set; }

    public virtual DbSet<category> categories { get; set; }

    public virtual DbSet<customer> customers { get; set; }

    public virtual DbSet<customer_recommendation> customer_recommendations { get; set; }

    public virtual DbSet<entry_mode> entry_modes { get; set; }

    public virtual DbSet<line_item> line_items { get; set; }

    public virtual DbSet<order> orders { get; set; }

    public virtual DbSet<product> products { get; set; }

    public virtual DbSet<product_recommendation> product_recommendations { get; set; }

    public virtual DbSet<rating> ratings { get; set; }

    public virtual DbSet<transaction_type> transaction_types { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:tbsheffdb.database.windows.net,1433;Initial Catalog=TylerTestDB;Persist Security Info=False;User ID=tbsheff;Password=prVZQcqP8zBiCkC;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

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

        modelBuilder.Entity<bank>(entity =>
        {
            entity.HasKey(e => e.bank_id);

            entity.ToTable("bank");

            entity.Property(e => e.name).HasMaxLength(50);
        });

        modelBuilder.Entity<card_type>(entity =>
        {
            entity.HasKey(e => e.card_type_id);

            entity.ToTable("card_type");

            entity.Property(e => e.description).HasMaxLength(50);
        });

        modelBuilder.Entity<category>(entity =>
        {
            entity.HasKey(e => e.category_id);

            entity.ToTable("category");

            entity.Property(e => e.description).HasMaxLength(50);
        });

        modelBuilder.Entity<customer>(entity =>
        {
            entity.HasKey(e => e.customer_ID);

            entity.ToTable("customer");

            entity.Property(e => e.customer_ID).ValueGeneratedNever();
            entity.Property(e => e.country_of_residence).HasMaxLength(50);
            entity.Property(e => e.first_name).HasMaxLength(50);
            entity.Property(e => e.gender).HasMaxLength(50);
            entity.Property(e => e.last_name).HasMaxLength(50);
        });

        modelBuilder.Entity<customer_recommendation>(entity =>
        {
            entity.HasKey(e => e.customer_id);

            entity.ToTable("customer_recommendation");

            entity.Property(e => e.customer_id).ValueGeneratedNever();
        });

        modelBuilder.Entity<entry_mode>(entity =>
        {
            entity.HasKey(e => e.entry_mode_id);

            entity.ToTable("entry_mode");

            entity.Property(e => e.description).HasMaxLength(50);
        });

        modelBuilder.Entity<line_item>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("line_item");
        });

        modelBuilder.Entity<order>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("order");

            entity.Property(e => e.country_of_transaction).HasMaxLength(50);
            entity.Property(e => e.day_of_week).HasMaxLength(50);
            entity.Property(e => e.shipping_address).HasMaxLength(50);
        });

        modelBuilder.Entity<product>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("product");

            entity.Property(e => e.description).HasMaxLength(2750);
            entity.Property(e => e.img_link).HasMaxLength(150);
            entity.Property(e => e.name).HasMaxLength(100);
            entity.Property(e => e.primary_color).HasMaxLength(50);
            entity.Property(e => e.secondary_color).HasMaxLength(50);
        });

        modelBuilder.Entity<product_recommendation>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("product_recommendation");
        });

        modelBuilder.Entity<rating>(entity =>
        {
            entity.HasKey(e => e.product_ID);

            entity.ToTable("rating");

            entity.Property(e => e.rating1).HasColumnName("rating");
        });

        modelBuilder.Entity<transaction_type>(entity =>
        {
            entity.HasKey(e => e.transaction_type_id);

            entity.ToTable("transaction_type");

            entity.Property(e => e.description).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
