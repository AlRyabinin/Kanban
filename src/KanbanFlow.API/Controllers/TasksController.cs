using KanbanFlow.API.Models;
using KanbanFlow.Application.Features.Tasks.Commands.CreateTask;
using KanbanFlow.Application.Features.Tasks.Commands.DeleteTask;
using KanbanFlow.Application.Features.Tasks.Commands.UpdateTask;
using KanbanFlow.Application.Features.Tasks.Commands.UpdateTaskPosition;
using KanbanFlow.Application.Features.Tasks.Queries.GetTasksByColumn;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KanbanFlow.API.Controllers;

/// <summary>
/// Контроллер для управления задачами на Канбан-доске.
/// Предоставляет API endpoints для создания, получения, обновления и удаления задач.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Конструктор контроллера задач.
    /// </summary>
    /// <param name="mediator">MediatR для отправки команд и запросов.</param>
    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Создаёт новую задачу в указанной колонке.
    /// </summary>
    /// <param name="request">Данные для создания задачи.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Созданная задача с присвоенным ID.</returns>
    /// <response code="201">Задача успешно создана.</response>
    /// <response code="400">Некорректные данные запроса (ошибка валидации).</response>
    /// <response code="404">Колонка не найдена.</response>
    [HttpPost]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskDto>> CreateTask(
        [FromBody] CreateTaskRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateTaskCommand(
            request.Title,
            request.Description,
            request.ColumnId,
            request.OrderIndex,
            request.DueDate
        );

        var result = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(
            nameof(GetTaskById),
            new { id = result.Id },
            result);
    }

    /// <summary>
    /// Получает все задачи в указанной колонке.
    /// Используется при загрузке Kanban-доски для отображения задач.
    /// </summary>
    /// <param name="columnId">Идентификатор колонки.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Список задач, отсортированных по позиции в колонке.</returns>
    /// <response code="200">Список задач успешно получен.</response>
    /// <response code="404">Колонка не найдена.</response>
    [HttpGet("column/{columnId:guid}")]
    [ProducesResponseType(typeof(List<TaskDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<TaskDto>>> GetTasksByColumn(
        Guid columnId,
        CancellationToken cancellationToken)
    {
        var query = new GetTasksByColumnQuery(columnId);
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Получает задачу по её идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор задачи.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Данные задачи.</returns>
    /// <response code="200">Задача найдена.</response>
    /// <response code="404">Задача не найдена.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskDto>> GetTaskById(
        Guid id,
        CancellationToken cancellationToken)
    {
        // TODO: Реализовать GetTaskByIdQuery
        return Ok(new TaskDto(id, "Заглушка", null, Guid.Empty, 0, null));
    }

    /// <summary>
    /// Обновляет позицию задачи (используется при Drag-and-Drop).
    /// Позволяет перемещать задачи между колонками и менять их порядок.
    /// </summary>
    /// <param name="request">Данные для обновления позиции.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <response code="204">Позиция задачи успешно обновлена.</response>
    /// <response code="400">Некорректные данные запроса.</response>
    /// <response code="404">Задача или колонка не найдена.</response>
    [HttpPut("position")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTaskPosition(
        [FromBody] UpdateTaskPositionRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateTaskPositionCommand(
            request.TaskId,
            request.NewColumnId,
            request.NewOrderIndex
        );

        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Обновить задачу.
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTask(Guid id, UpdateTaskCommand command, CancellationToken cancellationToken)
    {
        if(id != command.TaskId)
        {
            return BadRequest();
        }

        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Мягко удаляет задачу (Soft Delete).
    /// Задача помечается флагом IsDeleted и исключается из всех запросов.
    /// Физически запись остаётся в базе данных для возможного восстановления или аудита.
    /// </summary>
    /// <param name="id">Идентификатор задачи для удаления.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <response code="204">Задача успешно удалена.</response>
    /// <response code="404">Задача не найдена или уже удалена.</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTask(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteTaskCommand(id), cancellationToken);

        return NoContent();
    }
}