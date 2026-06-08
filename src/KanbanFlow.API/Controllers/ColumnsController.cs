using KanbanFlow.Application.Features.Columns.Commands.CreateColumn;
using KanbanFlow.Application.Features.Columns.Commands.DeleteColumn;
using KanbanFlow.Application.Features.Columns.Commands.UpdateColumn;
using KanbanFlow.Application.Features.Columns.Commands.UpdateColumnPosition;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KanbanFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ColumnsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ColumnsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Создать колонку.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateColumn(CreateColumnCommand command, CancellationToken cancellationToken)
    {
        var columnId = await _mediator.Send(command, cancellationToken);
        return Ok(columnId);
    }

    /// <summary>
    /// Обновить колонку.
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateColumn(Guid id, UpdateColumnCommand command, CancellationToken cancellationToken)
    {
        if (id != command.ColumnId)
        {
            return BadRequest();
        }

        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Обновить позицию колонки.
    /// </summary>
    [HttpPut("{id}/position")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateColumnPosition(Guid id, UpdateColumnPositionCommand command, CancellationToken cancellationToken)
    {
        if (id != command.ColumnId)
        {
            return BadRequest();
        }

        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Удалить колонку.
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteColumn(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteColumnCommand(id), cancellationToken);
        return NoContent();
    }
}