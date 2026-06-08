using KanbanFlow.Application.Interfaces;
using KanbanFlow.Domain.Entities;
using MediatR;

namespace KanbanFlow.Application.Features.Boards.Commands.CreateBoard;

public class CreateBoardCommandHandler : IRequestHandler<CreateBoardCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateBoardCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
    {
        var board = new Board
        {
            Name = request.Name
        };

        _context.Boards.Add(board);
        await _context.SaveChangesAsync(cancellationToken);

        var defaultColumns = new List<Column>
        {
            new Column
            {
                Name = "To Do",
                OrderIndex = 1,
                Color = null,
                BoardId = board.Id
            },
            new Column
            {
                Name = "In Progress",
                OrderIndex = 2,
                Color = "#10b981",
                BoardId = board.Id
            },
            new Column
            {
                Name = "Done",
                OrderIndex = 3,
                Color = null,
                BoardId = board.Id
            }
        };

        _context.Columns.AddRange(defaultColumns);
        await _context.SaveChangesAsync(cancellationToken);

        return board.Id;
    }
}