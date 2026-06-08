using KanbanFlow.Application.Features.Tasks.Commands.CreateTask;
using MediatR;

namespace KanbanFlow.Application.Features.Boards.Queries.GetBoardWithColumns;

/// <summary>
/// Запрос для получения полной структуры доски со всеми колонками и задачами.
/// Выполняет один запрос к базе данных с использованием Include для загрузки связанных данных.
/// </summary>
/// <param name="BoardId">Идентификатор доски для загрузки.</param>
public record GetBoardWithColumnsQuery(Guid BoardId) : IRequest<BoardWithColumnsDto>;