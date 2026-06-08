using MediatR;

namespace KanbanFlow.Application.Features.Boards.Commands.DeleteBoard;

public record DeleteBoardCommand(Guid BoardId) : IRequest;