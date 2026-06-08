using Microsoft.AspNetCore.Identity;

namespace KanbanFlow.Domain.Entities;

/// <summary>
/// Представляет пользователя системы.
/// Наследуется от IdentityUser для интеграции с ASP.NET Core Identity.
/// </summary>
public class User : IdentityUser
{
    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Фамилия пользователя.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Коллекция досок, принадлежащих пользователю.
    /// </summary>
    public ICollection<Board> Boards { get; set; } = new List<Board>();
}