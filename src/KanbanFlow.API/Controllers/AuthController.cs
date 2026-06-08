using KanbanFlow.Application.Features.Auth;
using KanbanFlow.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KanbanFlow.API.Controllers;

/// <summary>
/// Контроллер для управления аутентификацией пользователей.
/// Предоставляет endpoints для регистрации и входа.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="AuthController"/>.
    /// </summary>
    /// <param name="authService">Сервис аутентификации.</param>
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Регистрирует нового пользователя в системе.
    /// </summary>
    /// <param name="request">Данные для регистрации.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>
    /// <see cref="AuthResponse"/> с данными пользователя и JWT-токеном при успехе,
    /// или <see cref="BadRequestObjectResult"/> с описанием ошибки.
    /// </returns>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthResponse>> Register(
        [FromBody] RegisterRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _authService.RegisterAsync(request, cancellationToken);
            return Ok(response);
        }
        catch(Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Выполняет вход пользователя в систему.
    /// </summary>
    /// <param name="request">Учётные данные пользователя.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>
    /// <see cref="AuthResponse"/> с данными пользователя и JWT-токеном при успехе,
    /// или <see cref="UnauthorizedObjectResult"/> при неверных учётных данных.
    /// </returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponse>> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _authService.LoginAsync(request, cancellationToken);
            return Ok(response);
        }
        catch(Exception ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
}