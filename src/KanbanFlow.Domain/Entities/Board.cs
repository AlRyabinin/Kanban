using System.Text.Json.Serialization;

namespace KanbanFlow.Domain.Entities;

/// <summary>
/// Представляет доску Kanban, содержащую колонки и задачи.
/// </summary>
public class Board
{
    /// <summary>
    /// Уникальный идентификатор доски.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Название доски.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Дата и время создания доски в формате UTC.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Флаг мягкого удаления (true - удалено, false - активно).
    /// </summary>
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// Идентификатор пользователя-владельца доски.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Навигационное свойство к пользователю-владельцу.
    /// </summary>
    [JsonIgnore]
    public User User { get; set; } = null!;

    /// <summary>
    /// Коллекция колонок, принадлежащих доске.
    /// </summary>
    public ICollection<Column> Columns { get; set; } = new List<Column>();
}