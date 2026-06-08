using KanbanFlow.Application.Interfaces;
using KanbanFlow.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KanbanFlow.Application.Features.Boards.Commands.CreateBoard;

public class CreateBoardCommandHandler : IRequestHandler<CreateBoardCommand, Board>
{
    private readonly IApplicationDbContext _context;

    public CreateBoardCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Board> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
    {
        var board = new Board
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            UserId = request.UserId,
            CreatedAt = DateTime.UtcNow
        };

        // Создаём стандартные колонки
        board.Columns = new List<Column>
        {
            new Column { Id = Guid.NewGuid(), Name = "To Do", BoardId = board.Id, OrderIndex = 0 },
            new Column { Id = Guid.NewGuid(), Name = "In Progress", BoardId = board.Id, OrderIndex = 1 },
            new Column { Id = Guid.NewGuid(), Name = "Done", BoardId = board.Id, OrderIndex = 2 }
        };

        _context.Boards.Add(board);
        await _context.SaveChangesAsync(cancellationToken);

        return board;
    }
}