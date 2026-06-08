using KanbanFlow.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KanbanFlow.Application.Features.Tasks.Commands.DeleteTask;

/// <summary>
/// Обработчик команды мягкого удаления задачи.
/// Устанавливает флаг IsDeleted = true и обновляет метку времени UpdatedAt.
/// </summary>
public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand>
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Конструктор обработчика.
    /// </summary>
    /// <param name="context">Интерфейс контекста базы данных (реализация предоставляется через DI).</param>
    public DeleteTaskCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Метод, выполняющий бизнес-логику мягкого удаления задачи.
    /// </summary>
    /// <param name="request">Команда с идентификатором задачи.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    public async Task Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == request.TaskId, cancellationToken);

        if(task == null)
        {
            throw new InvalidOperationException($"Задача с ID {request.TaskId} не найдена или уже удалена.");
        }

        task.IsDeleted = true;
        task.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }
}