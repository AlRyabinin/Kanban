using KanbanFlow.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KanbanFlow.Application.Features.Columns.Commands.DeleteColumn;

public class DeleteColumnCommandHandler : IRequestHandler<DeleteColumnCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteColumnCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteColumnCommand request, CancellationToken cancellationToken)
    {
        var column = await _context.Columns
            .FirstOrDefaultAsync(c => c.Id == request.ColumnId, cancellationToken);

        if (column == null)
        {
            throw new KeyNotFoundException($"Колонка с ID {request.ColumnId} не найдена.");
        }

        _context.Columns.Remove(column);
        await _context.SaveChangesAsync(cancellationToken);
    }
}