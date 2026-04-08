using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SmartStock.API.Models.DTOs;
using SmartStock.API.Services;
using System.Security.Claims;

namespace SmartStock.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) => _authService = authService;

    /// <summary>Register a new user</summary>
    [HttpPost("register")]
    [AllowAnonymous]
    [EnableRateLimiting("AuthLimit")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto)
    {
        var result = await _authService.RegisterAsync(dto);
        return Ok(result);
    }

    /// <summary>Login with email and password</summary>
    [HttpPost("login")]
    [AllowAnonymous]
    [EnableRateLimiting("AuthLimit")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        return Ok(result);
    }

    /// <summary>Get all users (admin only)</summary>
    [HttpGet("users")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<UserDto>>> GetUsers()
    {
        var users = await _authService.GetUsersAsync();
        return Ok(users);
    }

    /// <summary>Update user role (admin only)</summary>
    [HttpPut("users/{userId}/role")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateRole(string userId, [FromBody] UpdateRoleDto dto)
    {
        await _authService.UpdateUserRoleAsync(userId, dto.Role);
        return NoContent();
    }

    /// <summary>Toggle user active status (admin only)</summary>
    [HttpPut("users/{userId}/toggle-status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ToggleStatus(string userId)
    {
        await _authService.ToggleUserStatusAsync(userId);
        return NoContent();
    }

    /// <summary>Get own profile</summary>
    [HttpGet("profile")]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("Identity could not be resolved.");
        var profile = await _authService.GetProfileAsync(userId);
        return Ok(profile);
    }

    /// <summary>Update own profile</summary>
    [HttpPut("profile")]
    [Authorize]
    public async Task<ActionResult<UserDto>> UpdateProfile([FromBody] UpdateProfileDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("Identity could not be resolved.");
        var updated = await _authService.UpdateProfileAsync(userId, dto);
        return Ok(updated);
    }

    /// <summary>Change own password</summary>
    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("Identity could not be resolved.");
        await _authService.ChangePasswordAsync(userId, dto);
        return Ok(new { message = "Contraseña actualizada exitosamente" });
    }
}
