using CSharpFunctionalExtensions;
using WarehouseManagement.SharedKernel;
using WarehouseManagement.SharedKernel.ValueObjects;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.BalanceManagement.Domain.Entities;

/// <summary> Сущность баланса </summary>
public class Balance : Entity<BalanceId>
{
    /// <summary> Конструктор для поддержки EF. Не использовать! </summary>
    private Balance(BalanceId id) : base(id) { }

    /// <summary> Конструктор баланса </summary>
    public Balance(
        ResourceId resourceId,
        UnitId unitId, 
        Stock resourceStock) : base(BalanceId.Create())
    {
        ResourceId = resourceId;
        UnitId = unitId;
        ResourceStock = resourceStock;
    }
    
    /// <summary> Идентификатор ресурса </summary>
    public ResourceId ResourceId { get; private set; }
    
    /// <summary> Идентификатор единицы измерения </summary>
    public UnitId UnitId { get; private set; }
    
    /// <summary> Количество ресурсов </summary>
    public Stock ResourceStock { get; private set; }

    /// <summary>
    /// Пополнение наличия ресурсов
    /// </summary>
    /// <param name="addedResourceStock">Добавляемое количество ресурсов</param>
    public UnitResult<Error> ReplenishResourceStock(int addedResourceStock)
    {
        var resourceStockResult = ResourceStock.Add(addedResourceStock);

        if (resourceStockResult.IsFailure)
            return resourceStockResult.Error;
        
        ResourceStock = resourceStockResult.Value;

        return UnitResult.Success<Error>();
    }
    
    /// <summary>
    /// Уменьшение наличия ресурсов
    /// </summary>
    /// <param name="subtractedResourceStock">Вычитаемое количество ресурсов</param>
    public UnitResult<Error> ReduceResourceStock(int subtractedResourceStock)
    {
        var resourceStockResult = ResourceStock.Subtract(subtractedResourceStock);

        if (resourceStockResult.IsFailure)
            return resourceStockResult.Error;
        
        ResourceStock = resourceStockResult.Value;

        return UnitResult.Success<Error>();
    }
}