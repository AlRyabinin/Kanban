using MediatR;

namespace KanbanFlow.Application.Features.Columns.Commands.UpdateColumn;

public record UpdateColumnCommand(
    Guid ColumnId,
    string Name,
    string? Color
) : IRequest;