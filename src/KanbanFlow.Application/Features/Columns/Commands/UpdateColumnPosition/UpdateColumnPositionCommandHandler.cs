using KanbanFlow.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KanbanFlow.Application.Features.Columns.Commands.UpdateColumnPosition;

public class UpdateColumnPositionCommandHandler : IRequestHandler<UpdateColumnPositionCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateColumnPositionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateColumnPositionCommand request, CancellationToken cancellationToken)
    {
        var column = await _context.Columns
            .FirstOrDefaultAsync(c => c.Id == request.ColumnId, cancellationToken);

        if (column == null)
        {
            throw new KeyNotFoundException($"Колонка с ID {request.ColumnId} не найдена.");
        }

        column.OrderIndex = request.NewOrderIndex;

        await _context.SaveChangesAsync(cancellationToken);
    }
}