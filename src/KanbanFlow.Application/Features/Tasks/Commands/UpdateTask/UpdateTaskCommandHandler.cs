using KanbanFlow.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KanbanFlow.Application.Features.Tasks.Commands.UpdateTask;

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTaskCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == request.TaskId, cancellationToken);

        if(task == null)
        {
            throw new KeyNotFoundException($"Задача с ID {request.TaskId} не найдена.");
        }

        task.Title = request.Title;
        task.Description = request.Description;
        task.DueDate = request.DueDate;

        await _context.SaveChangesAsync(cancellationToken);
    }
}