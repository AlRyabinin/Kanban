namespace KanbanFlow.API.Models;

/// <summary>
/// Модель запроса для обновления позиции задачи.
/// Используется при Drag-and-Drop на фронтенде.
/// </summary>
public class UpdateTaskPositionRequest
{
    /// <summary>
    /// Идентификатор перемещаемой задачи.
    /// </summary>
    public Guid TaskId { get; set; }

    /// <summary>
    /// Идентификатор новой колонки.
    /// </summary>
    public Guid NewColumnId { get; set; }

    /// <summary>
    /// Новая позиция задачи (дробное число для Fractional Indexing).
    /// </summary>
    public decimal NewOrderIndex { get; set; }
}