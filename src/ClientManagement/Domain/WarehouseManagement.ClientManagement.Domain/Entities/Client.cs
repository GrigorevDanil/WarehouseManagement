using CSharpFunctionalExtensions;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.Interfaces;
using WarehouseManagement.SharedKernel.ValueObjects;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.ClientManagement.Domain.Entities;

/// <summary> Сущность клиента </summary>
public class Client : Entity<ClientId>, IArchived
{
    /// <summary> Конструктор для поддержки EF. Не использовать! </summary>
    private Client(ClientId id) : base(id) { }

    /// <summary> Конструктор клиента </summary>
    public Client(Title title, Address address) : base(ClientId.Create())
    {
        Title = title;
        Address = address;
        IsArchived = IsArchived.Active;
    }
    
    /// <summary> Наименование клиента </summary>
    public Title Title { get; private set; }
    
    /// <summary> Адрес клиента </summary>
    public Address Address { get; private set; }
    
    /// <summary> Состояние клиента(Находится ли в архиве) </summary>
    public IsArchived IsArchived { get; private set; }

    /// <summary> Дата попадания клиента в архив </summary>
    public ArchivedAt ArchivedAt { get; private set; } = ArchivedAt.Empty;

    /// <summary> Переносит клиента в архив </summary>
    public void MoveToArchive()
    {
        IsArchived = IsArchived.Archived;
        ArchivedAt = ArchivedAt.Of(DateTime.UtcNow).Value;
    }

    /// <summary> Возвращает клиента из архива </summary>
    public void RestoreFromArchive()    
    {
        IsArchived = IsArchived.Active;
        ArchivedAt = ArchivedAt.Empty;
    }
    
    /// <summary>
    /// Обновляет информацию о клиента
    /// </summary>
    /// <param name="title">Название клиента</param>
    /// <param name="address">Адрес клиента</param>
    /// <returns></returns>
    public void UpdateMainInfo(Title title,Address address)
    {
        Title =  title;
        Address = address;
    }
}