using MediatR;

namespace KanbanFlow.Application.Features.Tasks.Commands.DeleteTask;

/// <summary>
/// Команда для мягкого удаления задачи (Soft Delete).
/// Задача не удаляется физически из базы данных, а помечается флагом IsDeleted.
/// Благодаря Global Query Filter в DbContext, она автоматически исключается из всех запросов.
/// </summary>
/// <param name="TaskId">Идентификатор задачи, которую нужно удалить.</param>
public record DeleteTaskCommand(Guid TaskId) : IRequest;