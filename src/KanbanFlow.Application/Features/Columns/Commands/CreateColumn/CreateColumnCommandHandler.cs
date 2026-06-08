using KanbanFlow.Application.Interfaces;
using KanbanFlow.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KanbanFlow.Application.Features.Columns.Commands.CreateColumn;

public class CreateColumnCommandHandler : IRequestHandler<CreateColumnCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateColumnCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateColumnCommand request, CancellationToken cancellationToken)
    {
        decimal orderIndex = request.OrderIndex;
        if (orderIndex == 0)
        {
            var maxOrder = await _context.Columns
                .Where(c => c.BoardId == request.BoardId)
                .MaxAsync(c => (decimal?)c.OrderIndex, cancellationToken) ?? 0;
            orderIndex = maxOrder + 1;
        }

        var column = new Column
        {
            BoardId = request.BoardId,
            Name = request.Name,
            Color = request.Color,
            OrderIndex = orderIndex
        };

        _context.Columns.Add(column);
        await _context.SaveChangesAsync(cancellationToken);

        return column.Id;
    }
}