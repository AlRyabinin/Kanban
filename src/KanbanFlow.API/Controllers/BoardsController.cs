using KanbanFlow.Application.Features.Boards.Commands.CreateBoard;
using KanbanFlow.Application.Features.Boards.Commands.DeleteBoard;
using KanbanFlow.Application.Features.Boards.Queries.GetBoardWithColumns;
using KanbanFlow.Application.Features.Tasks.Commands.CreateTask;
using KanbanFlow.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KanbanFlow.API.Controllers;

/// <summary>
/// Контроллер для управления досками.
/// Предоставляет API endpoints для получения структуры досок.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BoardsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Конструктор контроллера досок.
    /// </summary>
    /// <param name="mediator">MediatR для отправки команд и запросов.</param>
    /// <param name="context">Контекст базы данных.</param>
    public BoardsController(IMediator mediator, IApplicationDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    /// <summary>
    /// Получает список всех досок.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Список досок с ID и названием.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<object>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<object>>> GetAllBoards(CancellationToken cancellationToken)
    {
        var boards = await _context.Boards
            .Select(b => new { b.Id, b.Name })
            .ToListAsync(cancellationToken);

        return Ok(boards);
    }

    /// <summary>
    /// Получает полную структуру доски со всеми колонками и задачами.
    /// Используется при загрузке Kanban-доски на фронтенде.
    /// </summary>
    /// <param name="boardId">Идентификатор доски.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Полная структура доски с колонками и задачами.</returns>
    /// <response code="200">Доска успешно загружена.</response>
    /// <response code="404">Доска не найдена.</response>
    [HttpGet("{boardId:guid}/full")]
    [ProducesResponseType(typeof(BoardWithColumnsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BoardWithColumnsDto>> GetBoardWithColumns(
        Guid boardId,
        CancellationToken cancellationToken)
    {
        var query = new GetBoardWithColumnsQuery(boardId);
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Создать новую доску.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateBoard(CreateBoardCommand command, CancellationToken cancellationToken)
    {
        var boardId = await _mediator.Send(command, cancellationToken);
        return Created($"/api/Boards/{boardId}", boardId);
    }

    /// <summary>
    /// Удалить доску.
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBoard(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteBoardCommand(id), cancellationToken);
        return NoContent();
    }
}