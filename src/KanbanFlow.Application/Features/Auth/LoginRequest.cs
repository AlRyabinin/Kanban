namespace KanbanFlow.Application.Features.Auth;

/// <summary>
/// Запрос на аутентификацию пользователя.
/// </summary>
/// <param name="Email">Электронная почта пользователя.</param>
/// <param name="Password">Пароль пользователя.</param>
public record LoginRequest(
    string Email,
    string Password
);