namespace WarehouseManagement.IncomeProcessing.Contracts.Messaging;

public record DeleteIncomeResourceEvent(
    Guid ResourceId,
    Guid UnitId,
    int SubtractedResourceStock);