using KanbanFlow.Application.Interfaces;
using KanbanFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KanbanFlow.Infrastructure.Persistence;

/// <summary>
/// Контекст базы данных для приложения KanbanFlow.
/// Реализует интерфейс IApplicationDbContext, определённый в слое Application.
/// </summary>
public class KanbanDbContext : DbContext, IApplicationDbContext
{
    public KanbanDbContext(DbContextOptions<KanbanDbContext> options) : base(options) { }

    public DbSet<Board> Boards => Set<Board>();
    public DbSet<Column> Columns => Set<Column>();
    public DbSet<KanbanTask> Tasks => Set<KanbanTask>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<KanbanTask>().HasQueryFilter(t => !t.IsDeleted);
        builder.Entity<Column>().HasQueryFilter(c => !c.IsDeleted);
        builder.Entity<Board>().HasQueryFilter(b => !b.IsDeleted);

        builder.Entity<KanbanTask>()
            .HasOne(t => t.Column)
            .WithMany(c => c.Tasks)
            .HasForeignKey(t => t.ColumnId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<KanbanTask>()
            .HasIndex(t => new { t.ColumnId, t.OrderIndex });
    }
}