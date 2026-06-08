using KanbanFlow.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KanbanFlow.Application.Features.Boards.Commands.DeleteBoard;

public class DeleteBoardCommandHandler : IRequestHandler<DeleteBoardCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteBoardCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteBoardCommand request, CancellationToken cancellationToken)
    {
        var board = await _context.Boards
            .FirstOrDefaultAsync(b => b.Id == request.BoardId, cancellationToken);

        if(board == null)
        {
            throw new KeyNotFoundException($"Доска с ID {request.BoardId} не найдена.");
        }

        _context.Boards.Remove(board);
        await _context.SaveChangesAsync(cancellationToken);
    }
}