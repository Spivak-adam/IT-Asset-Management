using Microsoft.EntityFrameworkCore;
using IT_Asset.Models;

namespace IT_Asset.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<AssetHistory> AssetHistory { get; set; }
    public DbSet<Asset> Assets { get; set; }
    public DbSet<CheckoutRequest> CheckoutRequests { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CheckoutRequest>()
            .HasOne(cr => cr.ReviewedByUser)
            .WithMany()
            .HasForeignKey(cr => cr.ReviewedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CheckoutRequest>()
            .HasOne(cr => cr.AssignedAsset)
            .WithMany()
            .HasForeignKey(cr => cr.AssignedAssetId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Asset>()
            .HasOne(a => a.AssignedToUser)
            .WithMany()
            .HasForeignKey(a => a.AssignedToUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AssetHistory>()
            .HasOne(ah => ah.Asset)
            .WithMany()
            .HasForeignKey(ah => ah.AssetId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AssetHistory>()
            .HasOne(ah => ah.User)
            .WithMany()
            .HasForeignKey(ah => ah.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }

}