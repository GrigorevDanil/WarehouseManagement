namespace WarehouseManagement.IncomeProcessing.Contracts.Messaging;

public record AddIncomeResourceEvent(
    Guid ResourceId,
    Guid UnitId,
    int AddedResourceQuantity);