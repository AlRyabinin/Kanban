namespace KanbanFlow.Domain.Entities;

/// <summary>
/// Базовый абстрактный класс для всех сущностей доменной модели.
/// Содержит общие свойства, такие как идентификатор и метки времени.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Уникальный идентификатор сущности (GUID).
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Дата и время создания сущности в формате UTC.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Флаг мягкого удаления (Soft Delete). 
    /// Если true, сущность не удаляется физически, а исключается из выборок через глобальный фильтр EF Core.
    /// </summary>
    public bool IsDeleted { get; set; } = false;
}