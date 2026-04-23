using Microsoft.EntityFrameworkCore;
using MyFullstackApp.Domains.Entities.Capsule;
using MyFullstackApp.Domains.Entities.Category;
using MyFullstackApp.Domains.Entities.Moderation;
using MyFullstackApp.Domains.Entities.Product;
using MyFullstackApp.Domains.Entities.User;

namespace MyFullstackApp.DataAccess.Context;

public class AppDbContext : DbContext
{
    public DbSet<ProductData> Products { get; set; } = null!;
    public DbSet<CategoryData> Categories { get; set; } = null!;
    public DbSet<UserAccountData> UserAccounts { get; set; } = null!;
    public DbSet<TimeCapsuleData> TimeCapsules { get; set; } = null!;
    public DbSet<CapsuleLocationData> CapsuleLocations { get; set; } = null!;
    public DbSet<ModerationReportData> ModerationReports { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (DbSession.Provider.Equals("sqlserver", StringComparison.OrdinalIgnoreCase))
        {
            optionsBuilder.UseSqlServer(DbSession.ConnectionStrings);
            return;
        }

        optionsBuilder.UseSqlite(DbSession.ConnectionStrings);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductData>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserAccountData>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<TimeCapsuleData>()
            .HasOne(c => c.Owner)
            .WithMany(u => u.Capsules)
            .HasForeignKey(c => c.OwnerUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CapsuleLocationData>()
            .HasOne(l => l.Capsule)
            .WithOne(c => c.Location)
            .HasForeignKey<CapsuleLocationData>(l => l.CapsuleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ModerationReportData>()
            .HasOne(r => r.Capsule)
            .WithMany(c => c.Reports)
            .HasForeignKey(r => r.CapsuleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
