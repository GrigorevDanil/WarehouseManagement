using CSharpFunctionalExtensions;
using WarehouseManagement.SharedKernel.ValueObjects;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.IncomeProcessing.Domain.Entities;

/// <summary> Сущность ресурса поступления </summary>
public class IncomeResource : Entity<IncomeResourceId>
{
    /// <summary> Конструктор для поддержки EF. Не использовать! </summary>
    private IncomeResource(IncomeResourceId id) : base(id) { }

    /// <summary> Конструктор ресурса поступления</summary>
    public IncomeResource(
        IncomeDocumentId incomeDocumentId, 
        ResourceId resourceId,
        UnitId unitId, 
        Quantity resourceQuantity) : base(IncomeResourceId.Create())
    {
        IncomeDocumentId = incomeDocumentId;
        ResourceId = resourceId;
        UnitId = unitId;
        ResourceQuantity = resourceQuantity;
    }
    
    /// <summary> Идентификатор документа поступления </summary>
    public IncomeDocumentId IncomeDocumentId { get; private set; }
    
    /// <summary> Идентификатор ресурса </summary>
    public ResourceId ResourceId { get; private set; }
    
    /// <summary> Идентификатор единицы измерения </summary>
    public UnitId UnitId { get; private set; }
    
    /// <summary> Количество ресурсов </summary>
    public Quantity ResourceQuantity { get; private set; }

    /// <summary>
    /// Обновление информации о ресурсе в документе поступления
    /// </summary>
    /// <param name="resourceId">Идентификатор ресурса</param>
    /// <param name="unitId">Идентификатор единицы измерения</param>
    /// <param name="resourceQuantity">Количество ресурсов</param>
    public void UpdateMainInfo(ResourceId resourceId, UnitId unitId, Quantity resourceQuantity)
    {
        ResourceId =  resourceId;
        UnitId =  unitId;
        ResourceQuantity = resourceQuantity;
    }
}