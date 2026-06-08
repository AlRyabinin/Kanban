using KanbanFlow.Domain.Entities;
using KanbanFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KanbanFlow.Infrastructure;

/// <summary>
/// Класс для заполнения базы данных тестовыми данными.
/// </summary>
public static class DbSeeder
{
    /// <summary>
    /// Заполняет базу данных начальными данными (доски, колонки).
    /// </summary>
    public static async Task SeedAsync(KanbanDbContext context)
    {
        if(await context.Boards.AnyAsync())
        {
            return;
        }

        var board = new Board
        {
            Id = Guid.NewGuid(),
            Name = "Моя первая доска"
        };

        var columns = new List<Column>
        {
            new Column
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Name = "To Do",
                OrderIndex = 1,
                Color = null,
                BoardId = board.Id,
                Board = board
            },
            new Column
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                Name = "In Progress",
                OrderIndex = 2,
                Color = "#10b981",
                BoardId = board.Id,
                Board = board
            },
            new Column
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                Name = "Done",
                OrderIndex = 3,
                Color = null,
                BoardId = board.Id,
                Board = board
            }
        };

        context.Boards.Add(board);
        context.Columns.AddRange(columns);

        await context.SaveChangesAsync();
    }
}