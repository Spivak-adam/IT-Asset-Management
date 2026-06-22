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

    public async Task Login(string email, string Password)
    {
        
    }

    public async Task CheckoutRequest(int assetID)
    {
        
    }

    public async Task updateRequest(int requestID)
    {
        
    }
}