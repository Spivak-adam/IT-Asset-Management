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


    public async Task<CheckoutRequest> CreateCheckoutRequest(CreateCheckoutRequestDto request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.RequestedByUserId && u.IsActive);

        if (user == null)
        {
            throw new Exception("User not found or inactive.");
        }

        var checkoutRequest = new CheckoutRequest
        {
            RequestedByUserId = request.RequestedByUserId,
            AssetCategory = request.AssetCategory,
            Reason = request.Reason,
            Status = CheckoutRequestStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _context.CheckoutRequests.AddAsync(checkoutRequest);
        await _context.SaveChangesAsync();

        return checkoutRequest;
    }

    public async Task<List<AssetHistory>> GetAssetHistory(int assetId)
    {
        return await _context.AssetHistory
            .Where(h => h.AssetId == assetId)
            .OrderByDescending(h => h.CreatedAt)
            .ToListAsync();
    }

    public async Task updateRequest(int requestID)
    {

    }

    public async Task<List<Asset>> GetAllAssets()
    {
        return await _context.Assets.ToListAsync();
    }
}