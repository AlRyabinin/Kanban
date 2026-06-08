namespace KanbanFlow.Application.Features.Tasks.Commands.CreateTask;

/// <summary>
/// DTO, представляющий доску со всеми её колонками и задачами.
/// Используется для полной загрузки Kanban-доски на фронтенде.
/// </summary>
/// <param name="Id">Уникальный идентификатор доски.</param>
/// <param name="Name">Название доски.</param>
/// <param name="WorkspaceId">Идентификатор рабочего пространства.</param>
/// <param name="Columns">Список колонок с задачами, отсортированных по OrderIndex.</param>
public record BoardWithColumnsDto(
    Guid Id,
    string Name,
    List<ColumnWithTasksDto> Columns
);