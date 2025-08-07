using CSharpFunctionalExtensions;
using WarehouseManagement.SharedKernel.ValueObjects;
using WarehouseManagement.SharedKernel.ValueObjects.Ids;

namespace WarehouseManagement.OutcomeProcessing.Domain.Entities;

/// <summary> Сущность ресурса отгрузки </summary>
public class OutcomeResource: Entity<OutcomeResourceId>
{
    /// <summary> Конструктор для поддержки EF. Не использовать! </summary>
    private OutcomeResource(OutcomeResourceId id) : base(id) { }

    /// <summary> Конструктор ресурса отгрузки </summary>
    public OutcomeResource(
        OutcomeDocumentId outcomeDocumentId, 
        ResourceId resourceId,
        UnitId unitId, 
        Quantity resourceQuantity) : base(OutcomeResourceId.Create())
    {
        OutcomeDocumentId = outcomeDocumentId;
        ResourceId = resourceId;
        UnitId = unitId;
        ResourceQuantity = resourceQuantity;
    }
    
    /// <summary> Идентификатор документа отгрузки </summary>
    public OutcomeDocumentId OutcomeDocumentId { get; private set; }
    
    /// <summary> Идентификатор ресурса </summary>
    public ResourceId ResourceId { get; private set; }
    
    /// <summary> Идентификатор единицы измерения </summary>
    public UnitId UnitId { get; private set; }
    
    /// <summary> Количество ресурсов </summary>
    public Quantity ResourceQuantity { get; private set; }

    /// <summary>
    /// Обновление информации о ресурсе в документе отгрузки
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