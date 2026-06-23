using IT_Asset.Enums;
using IT_Asset.Models;
using Microsoft.EntityFrameworkCore;

namespace IT_Asset.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        await context.Database.MigrateAsync();

        if (await context.Users.AnyAsync())
        {
            return;
        }

        var admin = new User
        {
            Email = "admin@test.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
            Role = UserRole.Admin,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var manager = new User
        {
            Email = "manager@test.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
            Role = UserRole.AssetManager,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var employee = new User
        {
            Email = "employee@test.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!"),
            Role = UserRole.Employee,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        context.Users.AddRange(admin, manager, employee);
        await context.SaveChangesAsync();

        var assets = new List<Asset>
        {
            new Asset { AssetTag = "TL-LAP-001", Name = "Dell Latitude 5450", Category = "Laptop", SerialNumber = "LAP001", Status = AssetStatus.Assigned, Condition = AssetCondition.Good, AssignedToUserId = employee.Id, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Asset { AssetTag = "TL-LAP-002", Name = "Dell Latitude 5450", Category = "Laptop", SerialNumber = "LAP002", Status = AssetStatus.Available, Condition = AssetCondition.New, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Asset { AssetTag = "TL-LAP-003", Name = "MacBook Pro", Category = "Laptop", SerialNumber = "LAP003", Status = AssetStatus.Maintenance, Condition = AssetCondition.Damaged, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },

            new Asset { AssetTag = "TL-MON-001", Name = "Dell 27 Monitor", Category = "Monitor", SerialNumber = "MON001", Status = AssetStatus.Available, Condition = AssetCondition.Good, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Asset { AssetTag = "TL-MON-002", Name = "Dell 24 Monitor", Category = "Monitor", SerialNumber = "MON002", Status = AssetStatus.Available, Condition = AssetCondition.Good, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Asset { AssetTag = "TL-MON-003", Name = "LG Ultrawide", Category = "Monitor", SerialNumber = "MON003", Status = AssetStatus.Retired, Condition = AssetCondition.Fair, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },

            new Asset { AssetTag = "TL-PHN-001", Name = "iPhone 14", Category = "Phone", SerialNumber = "PHN001", Status = AssetStatus.Available, Condition = AssetCondition.Good, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Asset { AssetTag = "TL-PHN-002", Name = "Samsung Galaxy", Category = "Phone", SerialNumber = "PHN002", Status = AssetStatus.Available, Condition = AssetCondition.Good, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },

            new Asset { AssetTag = "TL-KEY-001", Name = "YubiKey 5", Category = "Security Key", SerialNumber = "KEY001", Status = AssetStatus.Available, Condition = AssetCondition.New, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new Asset { AssetTag = "TL-KEY-002", Name = "YubiKey 5", Category = "Security Key", SerialNumber = "KEY002", Status = AssetStatus.Available, Condition = AssetCondition.New, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        };

        context.Assets.AddRange(assets);
        await context.SaveChangesAsync();

        var requests = new List<CheckoutRequest>
        {
            new CheckoutRequest { RequestedByUserId = employee.Id, AssetCategory = "Laptop", Reason = "Need laptop for development work.", Status = CheckoutRequestStatus.Pending, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new CheckoutRequest { RequestedByUserId = employee.Id, AssetCategory = "Monitor", Reason = "Need second monitor.", Status = CheckoutRequestStatus.Pending, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new CheckoutRequest { RequestedByUserId = employee.Id, AssetCategory = "Security Key", Reason = "Need MFA security key.", Status = CheckoutRequestStatus.Pending, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },

            new CheckoutRequest { RequestedByUserId = employee.Id, ReviewedByUserId = manager.Id, AssetCategory = "Phone", Reason = "Need phone for testing.", Status = CheckoutRequestStatus.Approved, ApprovedAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new CheckoutRequest { RequestedByUserId = employee.Id, ReviewedByUserId = manager.Id, AssetCategory = "Monitor", Reason = "Need larger screen.", Status = CheckoutRequestStatus.Approved, ApprovedAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },

            new CheckoutRequest { RequestedByUserId = employee.Id, ReviewedByUserId = manager.Id, AssetCategory = "Tablet", Reason = "Wanted tablet for notes.", Status = CheckoutRequestStatus.Rejected, RejectedAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },

            new CheckoutRequest { RequestedByUserId = employee.Id, ReviewedByUserId = manager.Id, AssignedAssetId = assets[0].Id, AssetCategory = "Laptop", Reason = "Assigned work laptop.", Status = CheckoutRequestStatus.Fulfilled, ApprovedAt = DateTime.UtcNow, FulfilledAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },

            new CheckoutRequest { RequestedByUserId = employee.Id, ReviewedByUserId = manager.Id, AssetCategory = "Security Key", Reason = "Returned old key.", Status = CheckoutRequestStatus.Returned, ApprovedAt = DateTime.UtcNow, FulfilledAt = DateTime.UtcNow, ReturnedAt = DateTime.UtcNow, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        };

        context.CheckoutRequests.AddRange(requests);
        await context.SaveChangesAsync();

        context.AssetHistory.Add(new AssetHistory
        {
            AssetId = assets[0].Id,
            UserId = manager.Id,
            Action = "Seeded assigned laptop to employee@test.com",
            OldValue = "Available",
            NewValue = "Assigned",
            CreatedAt = DateTime.UtcNow
        });

        await context.SaveChangesAsync();
    }
}