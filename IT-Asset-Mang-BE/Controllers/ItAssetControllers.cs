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

    [HttpPost("auth/register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto request)
    {
        await _service.Register(request);
        return Ok("User registered successfully.");
    }

    [HttpPost("auth/login")]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        var response = await _service.Login(request.Email, request.Password);

        if (response == null)
        {
            return Unauthorized("Invalid email or password.");
        }

        return Ok(response);
    }

    [HttpPost("checkout-requests")]
    public async Task<IActionResult> CreateCheckoutRequest([FromBody] CreateCheckoutRequestDto request)
    {
        var checkoutRequest = await _service.CreateCheckoutRequest(request);

        return Ok(checkoutRequest);
    }

    [HttpGet("assets/{assetId}/history")]
    public async Task<IActionResult> GetAssetHistory(int assetId)
    {
        var history = await _service.GetAssetHistory(assetId);

        return Ok(history);
    }

    [HttpPost("update-request")]
    public async Task<IActionResult> UpdateRequest(int requestID)
    {
        await _service.updateRequest(requestID);
        return Ok();
    }

    [HttpGet("assets")]
    public async Task<IActionResult> GetAllAssets()
    {
        var assets = await _service.GetAllAssets();
        return Ok(assets);
    }
}
