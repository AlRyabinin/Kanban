using MediatR;

namespace KanbanFlow.Application.Features.Columns.Commands.CreateColumn;

public record CreateColumnCommand(
    Guid BoardId,
    string Name,
    string? Color,
    decimal OrderIndex
) : IRequest<Guid>;