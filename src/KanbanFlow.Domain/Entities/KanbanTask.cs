namespace KanbanFlow.Domain.Entities;

/// <summary>
/// Сущность, представляющая задачу на Канбан-доске.
/// </summary>
public class KanbanTask : BaseEntity
{
    /// <summary>
    /// Заголовок задачи.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Подробное описание задачи. Может быть пустым.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Идентификатор колонки, к которой привязана задача.
    /// </summary>
    public Guid ColumnId { get; set; }

    /// <summary>
    /// Навигационное свойство: колонка, содержащая данную задачу.
    /// </summary>
    public Column Column { get; set; } = null!;

    /// <summary>
    /// Порядковый номер задачи внутри колонки.
    /// Использует тип decimal (Fractional Indexing) для оптимизации Drag-and-Drop: 
    /// это позволяет вставлять задачу между двумя другими без пересчета индексов всех остальных задач в колонке.
    /// </summary>
    public decimal OrderIndex { get; set; }

    /// <summary>
    /// Дедлайн (срок выполнения) задачи. Может отсутствовать.
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Дата и время последнего обновления задачи в формате UTC.
    /// Используется для разрешения конфликтов при одновременном редактировании (Optimistic Concurrency).
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}