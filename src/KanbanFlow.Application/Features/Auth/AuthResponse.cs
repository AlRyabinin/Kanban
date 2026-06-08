namespace KanbanFlow.Application.Features.Auth;

/// <summary>
/// Ответ сервера после успешной регистрации или входа.
/// Содержит данные пользователя и JWT-токен.
/// </summary>
/// <param name="UserId">Уникальный идентификатор пользователя.</param>
/// <param name="Email">Электронная почта пользователя.</param>
/// <param name="FirstName">Имя пользователя.</param>
/// <param name="LastName">Фамилия пользователя.</param>
/// <param name="Token">JWT-токен для авторизации последующих запросов.</param>
public record AuthResponse(
    string UserId,
    string Email,
    string FirstName,
    string LastName,
    string Token
);