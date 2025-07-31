using CSharpFunctionalExtensions;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.ResourceManagement.Domain.Entities;

/// <summary> Сущность ресурса </summary>
public sealed class Resource : Entity<ResourceId>, IArchived
{
    /// <summary> Конструктор для поддержки EF. Не использовать! </summary>
    private Resource(ResourceId id) : base(id) { }

    /// <summary> Конструктор ресурса </summary>
    public Resource(Title title)
    {
        Id = ResourceId.Create();
        Title = title;
        IsArchived = IsArchived.Active;
    }
    
    /// <summary> Название ресурса </summary>
    public Title Title { get; private set; }
    
    /// <summary> Состояние ресурса(Находится ли в архиве) </summary>
    public IsArchived IsArchived { get; private set; }

    /// <summary> Дата попадания ресурса в архив </summary>
    public ArchivedAt ArchivedAt { get; private set; } = ArchivedAt.Empty;

    /// <summary> Переносит ресурс в архив </summary>
    public void MoveToArchive()
    {
        IsArchived = IsArchived.Archived;
        ArchivedAt = ArchivedAt.Of(DateTime.UtcNow).Value;
    }

    /// <summary> Возвращает ресурс из архива </summary>
    public void RestoreFromArchive()
    {
        IsArchived = IsArchived.Active;
        ArchivedAt = ArchivedAt.Empty;
    }

    /// <summary>
    /// Обновляет название ресурса
    /// </summary>
    /// <param name="title">Название ресурса</param>
    /// <returns></returns>
    public UnitResult<Error> UpdateTitle(string title)
    {
        var titleResult = Title.Of(title);
        
        if (titleResult.IsFailure)
            return titleResult.Error;
        
        Title = titleResult.Value;

        return UnitResult.Success<Error>();
    }
        
}
