using IT_Asset.Models;
using IT_Asset.Data;
using IT_Asset.Enums;
using IT_Asset.DTOs;
using Microsoft.EntityFrameworkCore;

namespace IT_Asset.Services;

public class ItAssetService
{
    private readonly AppDbContext _context;

    public ItAssetService(AppDbContext context)
    {
        _context = context;
    }

    public async Task Register(RegisterDto request)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (existingUser != null)
        {
            throw new Exception("A user with this email already exists.");
        }

        var hashPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User
        {
            Email = request.Email,
            PasswordHash = hashPassword,
            CreatedAt = DateTime.UtcNow,
            Role = request.Role,
            IsActive = true
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<LoginResponseDto?> Login(string email, string password)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            return null;
        }

        var validPassword = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

        if (!validPassword)
        {
            return null;
        }

        var token = Guid.NewGuid().ToString();

        return new LoginResponseDto
        {
            Id = user.Id,
            Token = token,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }

    public async Task Logout()
    {

    }

    private async Task AddAssetHistory(
        int assetId,
        int? userId,
        string action,
        string? oldValue,
        string? newValue)
    {
        await _context.AssetHistory.AddAsync(new AssetHistory
        {
            AssetId = assetId,
            UserId = userId,
            Action = action,
            OldValue = oldValue,
            NewValue = newValue,
            CreatedAt = DateTime.UtcNow
        });
    }

    public async Task<CheckoutRequestDto> CheckoutRequest(CreateCheckoutRequestDto request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.RequestedByUserId && u.IsActive);

        if (user == null)
        {
            throw new Exception("User not found or inactive.");
        }

        var asset = await _context.Assets
        .FirstOrDefaultAsync(a => a.Id == request.RequestedAssetId);

        if (asset == null)
        {
            throw new Exception("Asset not found.");
        }

        if (asset.Status != AssetStatus.Available)
        {
            throw new Exception("Asset is not available for checkout.");
        }

        var checkoutRequest = new CheckoutRequest
        {
            RequestedByUserId = request.RequestedByUserId,
            RequestedAssetId = request.RequestedAssetId,
            AssetCategory = request.AssetCategory,
            Reason = request.Reason,
            Status = CheckoutRequestStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await _context.CheckoutRequests.AddAsync(checkoutRequest);

        await _context.AssetHistory.AddAsync(new AssetHistory
        {
            AssetId = asset.Id,
            UserId = request.RequestedByUserId,
            Action = "Checkout Rejected",
            OldValue = CheckoutRequestStatus.Pending.ToString(),
            NewValue = CheckoutRequestStatus.Rejected.ToString(),
            CreatedAt = DateTime.UtcNow
        });


        // Update the asset status to Pending
        await _context.Assets
            .Where(a => a.Id == request.RequestedAssetId)
            .ExecuteUpdateAsync(setters =>
            setters.SetProperty(a => a.Status, AssetStatus.Pending));

        // Update the asset's updated at timestamp
        await _context.Assets
            .Where(a => a.Id == request.RequestedAssetId)
            .ExecuteUpdateAsync(setters =>
            setters.SetProperty(a => a.UpdatedAt, DateTime.UtcNow));

        await _context.SaveChangesAsync();

        return new CheckoutRequestDto
        {
            Id = checkoutRequest.Id,
            RequestedByUserId = checkoutRequest.RequestedByUserId,
            RequestedAssetId = checkoutRequest.RequestedAssetId,
            AssetCategory = checkoutRequest.AssetCategory,
            Reason = checkoutRequest.Reason,
            Status = checkoutRequest.Status,
            ReviewedByUserId = checkoutRequest.ReviewedByUserId,
            AssignedAssetId = checkoutRequest.AssignedAssetId,
            ApprovedAt = checkoutRequest.ApprovedAt,
            RejectedAt = checkoutRequest.RejectedAt,
            FulfilledAt = checkoutRequest.FulfilledAt,
            ReturnedAt = checkoutRequest.ReturnedAt,
            CreatedAt = checkoutRequest.CreatedAt,
            UpdatedAt = checkoutRequest.UpdatedAt
        };
    }

    public async Task<List<AssetHistory>> GetAssetHistory(int assetId)
    {
        return await _context.AssetHistory
            .Where(h => h.AssetId == assetId)
            .OrderByDescending(h => h.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Asset>> GetAllAssets()
    {
        return await _context.Assets.ToListAsync();
    }

    public async Task<List<AssetDto>> GetMyAssets(int userId)
    {
        return await _context.Assets
            .Where(a => a.AssignedToUserId == userId && !a.IsArchived)
            .Select(a => new AssetDto
            {
                Id = a.Id,
                AssetTag = a.AssetTag,
                Name = a.Name,
                Category = a.Category,
                SerialNumber = a.SerialNumber,
                Status = a.Status,
                Condition = a.Condition,
                AssignedToUserId = a.AssignedToUserId,
                CreatedAt = a.CreatedAt,
                UpdatedAt = a.UpdatedAt,
                IsArchived = a.IsArchived
            })
            .ToListAsync();
    }

    public async Task<List<CheckoutRequestDto>> GetCheckoutRequests()
    {
        return await _context.CheckoutRequests
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new CheckoutRequestDto
            {
                Id = r.Id,
                RequestedByUserId = r.RequestedByUserId,
                RequestedAssetId = r.RequestedAssetId,
                AssetCategory = r.AssetCategory,
                Reason = r.Reason,
                Status = r.Status,
                ReviewedByUserId = r.ReviewedByUserId,
                AssignedAssetId = r.AssignedAssetId,
                ApprovedAt = r.ApprovedAt,
                RejectedAt = r.RejectedAt,
                FulfilledAt = r.FulfilledAt,
                ReturnedAt = r.ReturnedAt,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            })
            .ToListAsync();
    }

    public async Task<CheckoutRequestDto> ApproveCheckoutRequest(int id)
    {
        var request = await _context.CheckoutRequests.FindAsync(id);

        if (request == null)
        {
            throw new Exception("Checkout request not found.");
        }

        if (request.Status != CheckoutRequestStatus.Pending)
        {
            throw new Exception("Only pending requests can be approved.");
        }

        if (request.RequestedAssetId == null)
        {
            throw new Exception("Requested asset not found on checkout request.");
        }

        var asset = await _context.Assets
            .FirstOrDefaultAsync(a => a.Id == request.RequestedAssetId);

        if (asset == null)
        {
            throw new Exception("Asset not found.");
        }

        if (asset.Status != AssetStatus.Pending)
        {
            throw new Exception("Asset is not available.");
        }

        var oldStatus = asset.Status.ToString();
        var oldAssignedUser = asset.AssignedToUserId?.ToString() ?? "Unassigned";

        request.Status = CheckoutRequestStatus.Approved;
        request.ApprovedAt = DateTime.UtcNow;
        request.AssignedAssetId = asset.Id;
        request.UpdatedAt = DateTime.UtcNow;

        asset.Status = AssetStatus.Assigned;
        asset.AssignedToUserId = request.RequestedByUserId;
        asset.UpdatedAt = DateTime.UtcNow;

        await _context.AssetHistory.AddAsync(new AssetHistory
        {
            AssetId = asset.Id,
            UserId = request.RequestedByUserId,
            Action = "Checkout Approved",
            OldValue = oldStatus,
            NewValue = AssetStatus.Assigned.ToString(),
            CreatedAt = DateTime.UtcNow
        });

        await _context.AssetHistory.AddAsync(new AssetHistory
        {
            AssetId = asset.Id,
            UserId = request.RequestedByUserId,
            Action = "Asset Assigned",
            OldValue = oldAssignedUser,
            NewValue = request.RequestedByUserId.ToString(),
            CreatedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();

        return new CheckoutRequestDto
        {
            Id = request.Id,
            RequestedByUserId = request.RequestedByUserId,
            RequestedAssetId = request.RequestedAssetId,
            AssetCategory = request.AssetCategory,
            Reason = request.Reason,
            Status = request.Status,
            ReviewedByUserId = request.ReviewedByUserId,
            AssignedAssetId = request.AssignedAssetId,
            ApprovedAt = request.ApprovedAt,
            RejectedAt = request.RejectedAt,
            FulfilledAt = request.FulfilledAt,
            ReturnedAt = request.ReturnedAt,
            CreatedAt = request.CreatedAt,
            UpdatedAt = request.UpdatedAt
        };
    }

    public async Task<CheckoutRequestDto> RejectCheckoutRequest(int id)
    {
        var request = await _context.CheckoutRequests.FindAsync(id);

        if (request == null)
        {
            throw new Exception("Checkout request not found.");
        }

        if (request.Status != CheckoutRequestStatus.Pending)
        {
            throw new Exception("Only pending requests can be rejected.");
        }

        if (request.RequestedAssetId == null)
        {
            throw new Exception("Requested asset not found on checkout request.");
        }

        var asset = await _context.Assets
            .FirstOrDefaultAsync(a => a.Id == request.RequestedAssetId);

        if (asset == null)
        {
            throw new Exception("Asset not found.");
        }

        request.Status = CheckoutRequestStatus.Rejected;
        request.RejectedAt = DateTime.UtcNow;
        request.UpdatedAt = DateTime.UtcNow;

        await _context.AssetHistory.AddAsync(new AssetHistory
        {
            AssetId = asset.Id,
            UserId = request.RequestedByUserId,
            Action = "Checkout Rejected",
            OldValue = CheckoutRequestStatus.Pending.ToString(),
            NewValue = CheckoutRequestStatus.Rejected.ToString(),
            CreatedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();

        return new CheckoutRequestDto
        {
            Id = request.Id,
            RequestedByUserId = request.RequestedByUserId,
            RequestedAssetId = request.RequestedAssetId,
            AssetCategory = request.AssetCategory,
            Reason = request.Reason,
            Status = request.Status,
            ReviewedByUserId = request.ReviewedByUserId,
            AssignedAssetId = request.AssignedAssetId,
            ApprovedAt = request.ApprovedAt,
            RejectedAt = request.RejectedAt,
            FulfilledAt = request.FulfilledAt,
            ReturnedAt = request.ReturnedAt,
            CreatedAt = request.CreatedAt,
            UpdatedAt = request.UpdatedAt
        };
    }
}