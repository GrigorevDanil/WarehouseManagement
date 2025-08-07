namespace WarehouseManagement.IncomeProcessing.Contracts.Messaging;

public record UpdateIncomeResourceEvent(
    Guid ResourceId,
    Guid UnitId,
    int UpdatedResourceQuantity);