using KanbanFlow.Domain.Entities;
using MediatR;

namespace KanbanFlow.Application.Features.Boards.Commands.CreateBoard;

public record CreateBoardCommand(string Name, string? UserId = null) : IRequest<Board>;