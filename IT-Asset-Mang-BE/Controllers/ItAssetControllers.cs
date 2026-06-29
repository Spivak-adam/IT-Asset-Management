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

    /*[HttpPost("auth/register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto request)
    {
        await _service.Register(request);
        return Ok("User registered successfully.");
    }*/

    [HttpPost("auth/login")]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        try
        {
            var response = await _service.Login(request.Email, request.Password);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    [HttpGet("checkout-requests")]
    public async Task<IActionResult> GetCheckoutRequests()
    {
        var requests = await _service.GetCheckoutRequests();
        return Ok(requests);
    }

    [HttpPost("checkout-requests")]
    public async Task<IActionResult> CheckoutRequest([FromBody] CreateCheckoutRequestDto request)
    {
        var checkoutRequest = await _service.CheckoutRequest(request);
        return Ok(checkoutRequest);
    }

    [HttpPatch("checkout-requests/{id}/approve")]
    public async Task<IActionResult> ApproveCheckoutRequest(
    int id,
    [FromBody] ApproveCheckoutRequestDto request)
    {
        var response = await _service.ApproveCheckoutRequest(
            id,
            request.ReviewedByUserId,
            request.AssignedAssetId
        );

        return Ok(response);
    }

    [HttpPatch("checkout-requests/{id}/reject/{reviewedByUserId}")]
    public async Task<IActionResult> RejectCheckoutRequest(int id, int reviewedByUserId)
    {
        return Ok(await _service.RejectCheckoutRequest(id, reviewedByUserId));
    }

    [HttpGet("assets/{assetId}/history")]
    public async Task<IActionResult> GetAssetHistory(int assetId)
    {
        var history = await _service.GetAssetHistory(assetId);

        return Ok(history);
    }

    [HttpGet("assets")]
    public async Task<IActionResult> GetAllAssets()
    {
        var assets = await _service.GetAllAssets();
        return Ok(assets);
    }

    [HttpGet("my-assets/{userId}")]
    public async Task<IActionResult> GetMyAssets(int userId)
    {
        return Ok(await _service.GetMyAssets(userId));
    }

    [HttpGet("assets/available/{category}")]
    public async Task<IActionResult> GetAvailableAssetsByCategory(string category)
    {
        var assets = await _service.GetAvailableAssetsByCategory(category);
        return Ok(assets);
    }

    [HttpGet("checkout-requests/my/{userId}")]
    public async Task<IActionResult> GetMyCheckoutRequests(int userId)
    {
        var requests = await _service.GetMyCheckoutRequests(userId);
        return Ok(requests);
    }

    [HttpPatch("checkout-requests/{id}/return")]
    public async Task<IActionResult> ReturnAsset(int id)
    {
        var response = await _service.ReturnAsset(id);
        return Ok(response);
    }

    [HttpPatch("assets/{id}/archive")]
    public async Task<IActionResult> ArchiveAsset(int id)
    {
        return Ok(await _service.ArchiveAsset(id));
    }

    [HttpPatch("assets/{id}/restore")]
    public async Task<IActionResult> RestoreAsset(int id)
    {
        return Ok(await _service.RestoreAsset(id));
    }

    [HttpPatch("assets/{id}/assign")]
    public async Task<IActionResult> AssignAsset(int id, [FromBody] AssignAssetDto request)
    {
        return Ok(await _service.AssignAsset(id, request.UserId));
    }

    [HttpPatch("assets/{id}/return")]
    public async Task<IActionResult> ReturnAssetFromAdmin(int id)
    {
        return Ok(await _service.ReturnAssetFromAdmin(id));
    }

    [HttpPatch("checkout-requests/{id}/request-return")]
    public async Task<IActionResult> RequestReturn(int id)
    {
        return Ok(await _service.RequestReturn(id));
    }

    [HttpPatch("assets/{assetId}/request-return/{userId}")]
    public async Task<IActionResult> RequestReturnByAsset(int assetId, int userId)
    {
        return Ok(await _service.RequestReturnByAsset(assetId, userId));
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _service.GetUsers();
        return Ok(users);
    }

    [HttpPatch("users/{id}/role")]
    public async Task<IActionResult> UpdateUserRole(int id, [FromBody] UpdateUserRoleDto request)
    {
        var user = await _service.UpdateUserRole(id, request.Role);
        return Ok(user);
    }

    [HttpPatch("users/{id}/active")]
    public async Task<IActionResult> UpdateUserActive(int id, [FromBody] UpdateUserActiveDto request)
    {
        var user = await _service.UpdateUserActive(id, request.IsActive);
        return Ok(user);
    }

}
