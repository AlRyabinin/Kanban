namespace KanbanFlow.Application.Features.Tasks.Commands.CreateTask;

/// <summary>
/// DTO, представляющий задачу после её создания.
/// Используется для передачи данных из слоя приложения в API.
/// </summary>
/// <param name="Id">Уникальный идентификатор задачи.</param>
/// <param name="Title">Заголовок задачи.</param>
/// <param name="Description">Описание задачи.</param>
/// <param name="ColumnId">Идентификатор колонки.</param>
/// <param name="OrderIndex">Порядковый номер для сортировки.</param>
/// <param name="DueDate">Дедлайн задачи.</param>
public record TaskDto(
    Guid Id,
    string Title,
    string? Description,
    Guid ColumnId,
    decimal OrderIndex,
    DateTime? DueDate
);