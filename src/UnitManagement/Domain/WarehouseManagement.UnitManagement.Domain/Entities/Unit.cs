using CSharpFunctionalExtensions;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.Interfaces;
using WarehouseManagement.SharedKernel.ValueObjects;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.UnitManagement.Domain.Entities;

/// <summary> Сущность единицы измерения </summary>
public class Unit : Entity<UnitId>, IArchived
{
    /// <summary> Конструктор для поддержки EF. Не использовать! </summary>
    private Unit(UnitId id) : base(id) { }

    /// <summary> Конструктор единицы измерения </summary>
    public Unit(Title title) : base(UnitId.Create())
    {
        Title = title;
        IsArchived = IsArchived.Active;
    }
    
    /// <summary> Название единицы измерения </summary>
    public Title Title { get; private set; }

    /// <summary> Состояние единицы измерения(Находится ли в архиве) </summary>
    public IsArchived IsArchived { get; private set; }

    /// <summary> Дата попадания единицы измерения в архив </summary>
    public ArchivedAt ArchivedAt { get; private set; } = ArchivedAt.Empty;

    /// <summary> Переносит единицу измерения в архив </summary>
    public void MoveToArchive()
    {
        IsArchived = IsArchived.Archived;
        ArchivedAt = ArchivedAt.Of(DateTime.UtcNow).Value;
    }

    /// <summary> Возвращает единицу измерения из архива </summary>
    public void RestoreFromArchive()
    {
        IsArchived = IsArchived.Active;
        ArchivedAt = ArchivedAt.Empty;
    }
    
    /// <summary>
    /// Обновляет информацию о единице измерения
    /// </summary>
    /// <param name="title">Название единицы измерения</param>
    /// <returns></returns>
    public void UpdateMainInfo(Title title)
    {
        Title = title;
    }
}