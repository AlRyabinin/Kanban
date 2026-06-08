using KanbanFlow.Application.Interfaces;
using KanbanFlow.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KanbanFlow.Application.Features.Tasks.Commands.CreateTask;

/// <summary>
/// Обработчик команды создания задачи.
/// Отвечает за преобразование команды в сущность домена и сохранение в базе данных.
/// </summary>
public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDto>
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Конструктор обработчика.
    /// </summary>
    /// <param name="context">Интерфейс контекста базы данных (реализация предоставляется через DI).</param>
    public CreateTaskCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Метод, выполняющий бизнес-логику создания задачи.
    /// </summary>
    public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        // Проверка существования колонки
        var columnExists = await _context.Columns
            .AnyAsync(c => c.Id == request.ColumnId && !c.IsDeleted, cancellationToken);

        if(!columnExists)
        {
            throw new InvalidOperationException($"Колонка с ID {request.ColumnId} не найдена или удалена.");
        }

        // Создание сущности домена
        var newTask = new KanbanTask
        {
            Title = request.Title,
            Description = request.Description,
            ColumnId = request.ColumnId,
            OrderIndex = request.OrderIndex,
            DueDate = request.DueDate
        };

        // Добавление в контекст и сохранение в БД
        _context.Tasks.Add(newTask);
        await _context.SaveChangesAsync(cancellationToken);

        // Возврат DTO
        return new TaskDto(
            newTask.Id,
            newTask.Title,
            newTask.Description,
            newTask.ColumnId,
            newTask.OrderIndex,
            newTask.DueDate
        );
    }
}