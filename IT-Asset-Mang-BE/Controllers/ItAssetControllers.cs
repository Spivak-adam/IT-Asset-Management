using IT_Asset.Models;
using IT_Asset.Services;
using IT_Asset.Enums;
using IT_Asset.DTOs;
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

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto request)
    {
        await _service.Register(request);
        return Ok("User registered successfully.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        await _service.Login(request.Email, request.Password);
        return Ok();
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> CheckoutRequest(int assetID)
    {
        await _service.CheckoutRequest(assetID);
        return Ok();
    }

    [HttpPost("update-request")]
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
