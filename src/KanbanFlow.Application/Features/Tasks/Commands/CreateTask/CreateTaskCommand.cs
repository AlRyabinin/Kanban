using MediatR;

namespace KanbanFlow.Application.Features.Tasks.Commands.CreateTask;

/// <summary>
/// Команда для создания новой задачи на Канбан-доске.
/// </summary>
/// <param name="Title">Заголовок задачи (обязательный).</param>
/// <param name="Description">Описание задачи (необязательный).</param>
/// <param name="ColumnId">Идентификатор колонки, куда будет добавлена задача.</param>
/// <param name="OrderIndex">Позиция задачи в колонке (используется дробное число для оптимизации Drag-and-Drop).</param>
/// <param name="DueDate">Дедлайн задачи (необязательный).</param>
public record CreateTaskCommand(
    string Title,
    string? Description,
    Guid ColumnId,
    decimal OrderIndex,
    DateTime? DueDate
) : IRequest<TaskDto>;