using FluentValidation;

namespace KanbanFlow.Application.Features.Tasks.Commands.CreateTask;

/// <summary>
/// Валидатор для команды создания задачи.
/// Проверяет корректность входных данных перед выполнением бизнес-логики.
/// </summary>
public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        // Заголовок обязателен и не должен быть длиннее 200 символов
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Заголовок задачи не может быть пустым.")
            .MaximumLength(200).WithMessage("Заголовок не может превышать 200 символов.");

        // Описание необязательно, но если есть, то не более 2000 символов
        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Описание не может превышать 2000 символов.");

        // ID колонки должен быть валидным GUID (не пустым)
        RuleFor(x => x.ColumnId)
            .NotEmpty().WithMessage("Необходимо указать корректный ID колонки.");

        // OrderIndex должен быть положительным числом
        RuleFor(x => x.OrderIndex)
            .GreaterThanOrEqualTo(0).WithMessage("Порядковый номер не может быть отрицательным.");
    }
}