using MediatR;

namespace KanbanFlow.Application.Features.Columns.Commands.UpdateColumnPosition;

public record UpdateColumnPositionCommand(
    Guid ColumnId,
    int NewOrderIndex
) : IRequest;