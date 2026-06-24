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

    var passwordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

    if (!passwordValid)
    {
        return null;
    }

    return new LoginResponseDto
    {
        Token = "temporary-token",
        Email = user.Email,
        Role = user.Role.ToString()
    };
}

    public async Task Logout()
    {

    }

    public async Task CheckoutRequest(int assetID)
    {

    }

    public async Task updateRequest(int requestID)
    {

    }

    public async Task<List<Asset>> GetAllAssets()
    {
        return await _context.Assets.ToListAsync();
    }
}