using MediatR;

namespace KanbanFlow.Application.Features.Tasks.Commands.UpdateTask;

public record UpdateTaskCommand(
    Guid TaskId,
    string Title,
    string? Description,
    DateTime? DueDate
) : IRequest;