using MediatR;

namespace KanbanFlow.Application.Features.Boards.Commands.CreateBoard;

public record CreateBoardCommand(string Name) : IRequest<Guid>;