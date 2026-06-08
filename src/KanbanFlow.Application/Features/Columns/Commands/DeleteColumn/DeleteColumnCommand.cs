using MediatR;

namespace KanbanFlow.Application.Features.Columns.Commands.DeleteColumn;

public record DeleteColumnCommand(Guid ColumnId) : IRequest;