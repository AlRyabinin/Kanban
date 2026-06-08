using KanbanFlow.Application.Features.Tasks.Commands.CreateTask;
using MediatR;

namespace KanbanFlow.Application.Features.Tasks.Queries.GetTasksByColumn;

/// <summary>
/// Запрос для получения всех задач в указанной колонке.
/// Используется при загрузке Kanban-доски для отображения задач в колонках.
/// </summary>
/// <param name="ColumnId">Идентификатор колонки, задачи которой нужно получить.</param>
public record GetTasksByColumnQuery(Guid ColumnId) : IRequest<List<TaskDto>>;