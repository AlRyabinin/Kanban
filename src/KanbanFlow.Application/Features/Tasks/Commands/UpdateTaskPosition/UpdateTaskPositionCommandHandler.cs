using KanbanFlow.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KanbanFlow.Application.Features.Tasks.Commands.UpdateTaskPosition;

public class UpdateTaskPositionCommandHandler : IRequestHandler<UpdateTaskPositionCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTaskPositionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateTaskPositionCommand request, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == request.TaskId, cancellationToken);

        if (task == null)
        {
            throw new KeyNotFoundException($"Задача с ID {request.TaskId} не найдена.");
        }

        task.ColumnId = request.NewColumnId;

        var tasksInColumn = await _context.Tasks
            .Where(t => t.ColumnId == request.NewColumnId && !t.IsDeleted)
            .OrderBy(t => t.OrderIndex)
            .ToListAsync(cancellationToken);

        int targetIndex = (int)request.NewOrderIndex - 1;

        tasksInColumn.RemoveAll(t => t.Id == request.TaskId);

        if (targetIndex < 0) targetIndex = 0;
        if (targetIndex > tasksInColumn.Count) targetIndex = tasksInColumn.Count;

        tasksInColumn.Insert(targetIndex, task);

        for (int i = 0; i < tasksInColumn.Count; i++)
        {
            tasksInColumn[i].OrderIndex = i + 1;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}