
namespace KanbanFlow.Domain.Entities;

/// <summary>
/// Колонка на Kanban-доске (например, "To Do", "In Progress", "Done").
/// Содержит задачи и имеет цветовой индикатор.
/// </summary>
public class Column : BaseEntity
{
    /// <summary>
    /// Название колонки.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Порядковый номер колонки на доске.
    /// </summary>
    public decimal OrderIndex { get; set; }

    /// <summary>
    /// Цвет заголовка колонки в формате HEX (например, "#3b82f6").
    /// </summary>
    public string? Color { get; set; }

    /// <summary>
    /// Идентификатор доски, к которой принадлежит колонка.
    /// </summary>
    public Guid BoardId { get; set; }

    /// <summary>
    /// Навигационное свойство: доска, которой принадлежит колонка.
    /// </summary>
    public Board Board { get; set; } = null!;

    /// <summary>
    /// Навигационное свойство: задачи в этой колонке.
    /// </summary>
    public ICollection<KanbanTask> Tasks { get; set; } = new List<KanbanTask>();
}