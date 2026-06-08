using KanbanFlow.Application.Features.Auth;

namespace KanbanFlow.Application.Interfaces;

/// <summary>
/// Интерфейс сервиса аутентификации.
/// Отвечает за регистрацию и вход пользователей.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Регистрирует нового пользователя в системе.
    /// </summary>
    /// <param name="request">Данные для регистрации (email, пароль, имя, фамилия).</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Объект <see cref="AuthResponse"/> с данными пользователя и JWT-токеном.</returns>
    /// <exception cref="Exception">
    /// Выбрасывается, если email уже занят или пароль не соответствует требованиям.
    /// </exception>
    Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Выполняет вход пользователя в систему.
    /// </summary>
    /// <param name="request">Учётные данные пользователя (email и пароль).</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Объект <see cref="AuthResponse"/> с данными пользователя и JWT-токеном.</returns>
    /// <exception cref="Exception">
    /// Выбрасывается, если пользователь не найден или пароль неверный.
    /// </exception>
    Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
}