using IT_Asset.Models;
using IT_Asset.Services;
using Microsoft.AspNetCore.Mvc;

namespace IT_Asset.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItAssetController : ControllerBase
{
    private readonly ItAssetService _service;
    
    public ItAssetController(ItAssetService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Login(string email, string password)
    {
        await _service.Login(email, password);
        return Ok();
    }

    public async Task<IActionResult> CheckoutRequest(int assetID)
    {
        await _service.CheckoutRequest(assetID);
        return Ok();
    }

    public async Task<IActionResult> UpdateRequest(int requestID)
    {
        await _service.updateRequest(requestID);
        return Ok();
    }

    public async Task<IActionResult> GetAllAssets()
    {
        var assets = await _service.GetAllAssets();
        return Ok(assets);
    }
}
