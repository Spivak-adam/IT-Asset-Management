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
            return Unauthorized("Invalid email or password");
        }

        return Ok(response);
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

    [HttpGet("assets")]
    public async Task<IActionResult> GetAllAssets()
    {
        var assets = await _service.GetAllAssets();
        return Ok(assets);
    }
}
