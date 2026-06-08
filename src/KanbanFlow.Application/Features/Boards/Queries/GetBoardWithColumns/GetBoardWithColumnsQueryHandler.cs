using KanbanFlow.Application.Features.Tasks.Commands.CreateTask;
using KanbanFlow.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskEntity = KanbanFlow.Domain.Entities.KanbanTask;

namespace KanbanFlow.Application.Features.Boards.Queries.GetBoardWithColumns;

/// <summary>
/// Обработчик запроса получения полной структуры доски.
/// Использует Include для загрузки колонок и задач в одном запросе (избегает N+1 проблемы).
/// </summary>
public class GetBoardWithColumnsQueryHandler : IRequestHandler<GetBoardWithColumnsQuery, BoardWithColumnsDto>
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Конструктор обработчика.
    /// </summary>
    /// <param name="context">Интерфейс контекста базы данных.</param>
    public GetBoardWithColumnsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Метод, выполняющий бизнес-логику загрузки доски.
    /// </summary>
    public async Task<BoardWithColumnsDto> Handle(GetBoardWithColumnsQuery request, CancellationToken cancellationToken)
    {
        var board = await _context.Boards
            .Include(b => b.Columns)
                .ThenInclude(c => c.Tasks)
            .FirstOrDefaultAsync(b => b.Id == request.BoardId, cancellationToken);

        if (board == null)
        {
            throw new InvalidOperationException($"Доска с ID {request.BoardId} не найдена.");
        }

        return new BoardWithColumnsDto(
            board.Id,
            board.Name,
            board.Columns
            .OrderBy(c => c.OrderIndex)
            .Select(c => new ColumnWithTasksDto(
                c.Id,
                c.Name,
                c.Color,
                c.OrderIndex,
                c.Tasks
                    .OrderBy(t => t.OrderIndex)
                    .Select(t => new TaskDto(
                        t.Id,
                        t.Title,
                        t.Description,
                        t.ColumnId,
                        t.OrderIndex,
                        t.DueDate
                    )).ToList()
            )).ToList());
    }
}