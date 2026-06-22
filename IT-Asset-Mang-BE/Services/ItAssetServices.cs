using IT_Asset.Models;
using IT_Asset.Data;
using Microsoft.EntityFrameworkCore;

namespace IT_Asset.Services;

public class ItAssetService
{
    private readonly AppDbContext _context;

    public ItAssetService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task Register(string email, string password)
    {
        var hashPassword = BCrypt.Net.BCrypt.HashPassword(password);
        await _context.Users.AddAsync(new User { Email = email, PasswordHash = hashPassword });
        await _context.SaveChangesAsync();
    }

    public async Task Login(string email, string Password)
    {
        await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == Password);
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