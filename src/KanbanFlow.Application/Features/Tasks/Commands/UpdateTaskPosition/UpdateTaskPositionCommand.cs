using MediatR;

namespace KanbanFlow.Application.Features.Tasks.Commands.UpdateTaskPosition;

/// <summary>
/// Команда для обновления позиции задачи (используется при Drag-and-Drop).
/// Позволяет перемещать задачи между колонками и менять их порядок внутри колонки.
/// </summary>
/// <param name="TaskId">Идентификатор перемещаемой задачи.</param>
/// <param name="NewColumnId">Идентификатор новой колонки.</param>
/// <param name="NewOrderIndex">Новая позиция задачи в колонке (дробное число для Fractional Indexing).</param>
public record UpdateTaskPositionCommand(
    Guid TaskId,
    Guid NewColumnId,
    decimal NewOrderIndex
) : IRequest;