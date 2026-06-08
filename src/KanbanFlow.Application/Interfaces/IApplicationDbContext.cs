using KanbanFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KanbanFlow.Application.Interfaces;

/// <summary>
/// Интерфейс контекста базы данных.
/// Определяет контракт, который должен реализовать слой Infrastructure.
/// Позволяет слою Application работать с БД, не зная о конкретной реализации (EF Core, PostgreSQL).
/// </summary>
public interface IApplicationDbContext
{
    /// <summary>
    /// Набор сущностей задач.
    /// </summary>
    DbSet<KanbanTask> Tasks { get; }

    /// <summary>
    /// Набор сущностей колонок.
    /// </summary>
    DbSet<Column> Columns { get; }

    /// <summary>
    /// Набор сущностей досок.
    /// </summary>
    DbSet<Board> Boards { get; }

    /// <summary>
    /// Сохраняет все изменения в базе данных асинхронно.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Количество записей, записанных в базу данных.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}