namespace KanbanFlow.API.Models;

/// <summary>
/// Модель запроса для создания задачи.
/// Используется для получения данных от клиента (фронтенда).
/// </summary>
public class CreateTaskRequest
{
    /// <summary>
    /// Заголовок задачи (обязательный, макс. 200 символов).
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Описание задачи (необязательный, макс. 2000 символов).
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Идентификатор колонки, куда будет добавлена задача.
    /// </summary>
    public Guid ColumnId { get; set; }

    /// <summary>
    /// Порядковый номер задачи в колонке (для сортировки).
    /// </summary>
    public decimal OrderIndex { get; set; }

    /// <summary>
    /// Дедлайн задачи (необязательный).
    /// </summary>
    public DateTime? DueDate { get; set; }
}