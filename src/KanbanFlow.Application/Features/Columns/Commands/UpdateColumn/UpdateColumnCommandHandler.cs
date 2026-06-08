using KanbanFlow.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KanbanFlow.Application.Features.Columns.Commands.UpdateColumn;

public class UpdateColumnCommandHandler : IRequestHandler<UpdateColumnCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateColumnCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateColumnCommand request, CancellationToken cancellationToken)
    {
        var column = await _context.Columns
            .FirstOrDefaultAsync(c => c.Id == request.ColumnId, cancellationToken);

        if (column == null)
        {
            throw new KeyNotFoundException($"Колонка с ID {request.ColumnId} не найдена.");
        }

        column.Name = request.Name;
        column.Color = request.Color;

        await _context.SaveChangesAsync(cancellationToken);
    }
}