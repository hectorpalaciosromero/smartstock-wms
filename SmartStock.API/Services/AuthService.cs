using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmartStock.API.Models.DTOs;
using SmartStock.API.Models.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartStock.API.Services;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
    Task<List<UserDto>> GetUsersAsync();
    Task UpdateUserRoleAsync(string userId, string newRole);
    Task ToggleUserStatusAsync(string userId);
    Task<UserDto> GetProfileAsync(string userId);
    Task<UserDto> UpdateProfileAsync(string userId, UpdateProfileDto dto);
    Task ChangePasswordAsync(string userId, ChangePasswordDto dto);
}

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _config;
    private readonly ILogger<AuthService> _logger;

    public AuthService(UserManager<ApplicationUser> userManager, IConfiguration config, ILogger<AuthService> logger)
    {
        _userManager = userManager;
        _config = config;
        _logger = logger;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FullName = dto.FullName,
            Phone = dto.Phone,
            Department = dto.Department,
            Position = dto.Position,
            BirthDate = dto.BirthDate
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new ArgumentException($"Registration failed: {errors}");
        }

        var role = dto.Role ?? "Viewer";
        await _userManager.AddToRoleAsync(user, role);

        _logger.LogInformation("New user registered: {Email} with role {Role}", dto.Email, role);

        return await GenerateTokenResponse(user);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email)
            ?? throw new UnauthorizedAccessException("Invalid credentials");

        if (!await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            _logger.LogWarning("Failed login attempt for: {Email}", dto.Email);
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        if (!user.IsActive)
            throw new UnauthorizedAccessException("Account is deactivated");

        _logger.LogInformation("User logged in: {Email}", dto.Email);
        return await GenerateTokenResponse(user);
    }

    public async Task<List<UserDto>> GetUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        var result = new List<UserDto>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            result.Add(new UserDto(
                user.Id, user.FullName, user.Email!, user.AvatarUrl,
                user.Phone, user.Department, user.Position, user.BirthDate,
                roles.FirstOrDefault() ?? "Viewer", user.IsActive, user.CreatedAt
            ));
        }

        return result;
    }

    public async Task UpdateUserRoleAsync(string userId, string newRole)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new KeyNotFoundException("Usuario no encontrado");

        var validRoles = new[] { "Admin", "Almacenero", "Supervisor", "Viewer" };
        if (!validRoles.Contains(newRole))
            throw new ArgumentException("Rol inválido especificado.");

        var currentRoles = await _userManager.GetRolesAsync(user);
        var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
        if (!removeResult.Succeeded)
            throw new InvalidOperationException("Falló la remoción de roles actuales.");

        var assignResult = await _userManager.AddToRoleAsync(user, newRole);
        if (!assignResult.Succeeded)
            throw new InvalidOperationException("Falló la asignación del nuevo rol.");

        _logger.LogInformation("Rol actualizado para {Email} a {Role}", user.Email, newRole);
    }

    public async Task ToggleUserStatusAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new KeyNotFoundException("Usuario no encontrado");

        user.IsActive = !user.IsActive;
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            throw new InvalidOperationException("Falló el cambio de estado del usuario.");

        _logger.LogInformation("Estado del usuario {Email} cambiado a IsActive={IsActive}", user.Email, user.IsActive);
    }

    public async Task<UserDto> GetProfileAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new KeyNotFoundException("Usuario no encontrado");
        var roles = await _userManager.GetRolesAsync(user);
        return new UserDto(
            user.Id, user.FullName, user.Email!, user.AvatarUrl,
            user.Phone, user.Department, user.Position, user.BirthDate,
            roles.FirstOrDefault() ?? "Viewer", user.IsActive, user.CreatedAt
        );
    }

    public async Task<UserDto> UpdateProfileAsync(string userId, UpdateProfileDto dto)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new KeyNotFoundException("Usuario no encontrado");

        user.FullName      = dto.FullName;
        user.Phone         = dto.Phone;
        user.Department    = dto.Department;
        user.Position      = dto.Position;
        user.BirthDate     = dto.BirthDate;
        user.AvatarUrl     = dto.AvatarUrl;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new InvalidOperationException("Error actualizando perfil.");

        _logger.LogInformation("Perfil actualizado para {Email}", user.Email);

        var roles = await _userManager.GetRolesAsync(user);
        return new UserDto(
            user.Id, user.FullName, user.Email!, user.AvatarUrl,
            user.Phone, user.Department, user.Position, user.BirthDate,
            roles.FirstOrDefault() ?? "Viewer", user.IsActive, user.CreatedAt
        );
    }

    public async Task ChangePasswordAsync(string userId, ChangePasswordDto dto)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new KeyNotFoundException("Usuario no encontrado");

        var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new ArgumentException($"Error cambiando contraseña: {errors}");
        }

        _logger.LogInformation("Password changed for user {Email}", user.Email);
    }

    private async Task<AuthResponseDto> GenerateTokenResponse(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Name, user.FullName),
            new("uid", user.Id)
        };

        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expirationHours = int.TryParse(_config["Jwt:ExpirationHours"], out var hours) ? hours : 4;
        var expiration = DateTime.UtcNow.AddHours(expirationHours);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: creds
        );

        return new AuthResponseDto(
            Token: new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken: Guid.NewGuid().ToString(),
            UserId: user.Id,
            FullName: user.FullName,
            Email: user.Email!,
            Role: roles.FirstOrDefault() ?? "Viewer",
            Expiration: expiration,
            AvatarUrl: user.AvatarUrl,
            Phone: user.Phone,
            Department: user.Department,
            Position: user.Position,
            BirthDate: user.BirthDate
        );
    }
}
