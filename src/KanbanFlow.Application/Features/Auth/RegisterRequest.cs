namespace KanbanFlow.Application.Features.Auth;

/// <summary>
/// Запрос на регистрацию нового пользователя.
/// </summary>
/// <param name="Email">Электронная почта пользователя (используется как логин).</param>
/// <param name="Password">Пароль пользователя (минимум 6 символов).</param>
/// <param name="FirstName">Имя пользователя.</param>
/// <param name="LastName">Фамилия пользователя.</param>
public record RegisterRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName
);