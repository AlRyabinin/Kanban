namespace KanbanFlow.Application.Features.Tasks.Commands.CreateTask;

/// <summary>
/// DTO, представляющий колонку с её задачами.
/// Используется для загрузки полной структуры доски.
/// </summary>
/// <param name="Id">Уникальный идентификатор колонки.</param>
/// <param name="Name">Название колонки (например, "To Do", "In Progress").</param>
/// <param name="OrderIndex">Порядковый номер колонки на доске.</param>
/// <param name="Tasks">Список задач в этой колонке, отсортированных по OrderIndex.</param>
public record ColumnWithTasksDto(
    Guid Id,
    string Name,
    string? Color,
    decimal OrderIndex,
    List<TaskDto> Tasks
);