using KanbanFlow.Application.Features.Tasks.Commands.CreateTask;
using KanbanFlow.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KanbanFlow.Application.Features.Tasks.Queries.GetTasksByColumn;

/// <summary>
/// Обработчик запроса получения задач по колонке.
/// Выполняет чтение данных из базы данных с применением фильтрации и сортировки.
/// </summary>
public class GetTasksByColumnQueryHandler : IRequestHandler<GetTasksByColumnQuery, List<TaskDto>>
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Конструктор обработчика запроса.
    /// </summary>
    /// <param name="context">Интерфейс контекста базы данных (реализация предоставляется через DI).</param>
    public GetTasksByColumnQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Метод, выполняющий бизнес-логику получения задач.
    /// </summary>
    /// <param name="request">Запрос с идентификатором колонки.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Список DTO задач, отсортированных по OrderIndex.</returns>
    public async Task<List<TaskDto>> Handle(GetTasksByColumnQuery request, CancellationToken cancellationToken)
    {
        var tasks = await _context.Tasks
            .Where(t => t.ColumnId == request.ColumnId)
            .OrderBy(t => t.OrderIndex) 
            .ToListAsync(cancellationToken);

        return tasks.Select(t => new TaskDto(
            t.Id,
            t.Title,
            t.Description,
            t.ColumnId,
            t.OrderIndex,
            t.DueDate
        )).ToList();
    }
}