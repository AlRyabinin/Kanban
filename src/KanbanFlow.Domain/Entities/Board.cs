namespace KanbanFlow.Domain.Entities;

/// <summary>
/// Сущность, представляющая Канбан-доску в рамках рабочего пространства.
/// </summary>
public class Board : BaseEntity
{
    /// <summary>
    /// Название доски.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Коллекция колонок, входящих в состав доски.
    /// </summary>
    public ICollection<Column> Columns { get; set; } = new List<Column>();
}