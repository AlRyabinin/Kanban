using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using KanbanFlow.Application.Features.Auth;
using KanbanFlow.Application.Interfaces;
using KanbanFlow.Application.Settings;
using KanbanFlow.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace KanbanFlow.Infrastructure.Services;

/// <summary>
/// Реализация сервиса аутентификации.
/// Отвечает за регистрацию, вход и генерацию JWT-токенов.
/// </summary>
public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly JwtSettings _jwtSettings;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AuthService"/>.
    /// </summary>
    /// <param name="userManager">Менеджер пользователей ASP.NET Identity.</param>
    /// <param name="jwtSettings">Настройки JWT из конфигурации.</param>
    public AuthService(
        UserManager<User> userManager,
        IOptions<JwtSettings> jwtSettings)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value;
    }

    /// <inheritdoc />
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if(!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new Exception($"Ошибка регистрации: {errors}");
        }

        var token = await GenerateTokenAsync(user);

        return new AuthResponse(
            user.Id,
            user.Email!,
            user.FirstName,
            user.LastName,
            token
        );
    }

    /// <inheritdoc />
    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if(user == null)
        {
            throw new Exception("Пользователь не найден");
        }

        var isValid = await _userManager.CheckPasswordAsync(user, request.Password);

        if(!isValid)
        {
            throw new Exception("Неверный пароль");
        }

        var token = await GenerateTokenAsync(user);

        return new AuthResponse(
            user.Id,
            user.Email!,
            user.FirstName,
            user.LastName,
            token
        );
    }

    /// <summary>
    /// Генерирует JWT-токен для указанного пользователя.
    /// </summary>
    /// <param name="user">Пользователь, для которого генерируется токен.</param>
    /// <returns>Строковое представление JWT-токена.</returns>
    private async Task<string> GenerateTokenAsync(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!),
            new("FirstName", user.FirstName),
            new("LastName", user.LastName)
        };

        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}